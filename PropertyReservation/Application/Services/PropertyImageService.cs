using AutoMapper;
using Infrastructure.Repositories;
using Application.DTOs.PropertyImage;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    // Administra la carga, consulta y eliminación de imágenes de propiedades.
    public class PropertyImageService
    {
        private readonly PropertyImageRepository _imageRepository;
        private readonly PropertyRepository _propertyRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly CurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public PropertyImageService(
            PropertyImageRepository imageRepository,
            PropertyRepository propertyRepository,
            IWebHostEnvironment webHostEnvironment,
            CurrentUserService currentUserService,
            IMapper mapper
        )
        {
            _imageRepository = imageRepository;
            _propertyRepository = propertyRepository;
            _environment = webHostEnvironment;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<PropertyImageResponseDTO>> GetImagesByPropertyAsync(int propertyId)
        {
            var propertyExists = await _propertyRepository.ExistsAsync(propertyId);
            if (!propertyExists)
            {
                throw new ArgumentException("La propiedad no existe.");
            }
            var images = await _imageRepository.GetByPropertyIdAsync(propertyId);
            return images.Select(img => _mapper.Map<PropertyImageResponseDTO>(img)).ToList();
        }

        public async Task<List<PropertyImageResponseDTO>> UploadPropertyImagesAsync(int propertyId, List<IFormFile> files, HttpRequest request)
        {
            var ownerId = _currentUserService.UserId;

            // Validar que la propiedad exista
            var property = await _propertyRepository.GetByIdAsync(propertyId);
            if (property == null)
                throw new KeyNotFoundException("La propiedad especificada no existe.");

            // Validar que el usuario actual sea el propietario de la propiedad
            if (property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tienes permiso para modificar las imagenes de esta propiedad.");

            // Validar que se hayan proporcionado archivos
            if (files == null || files.Count() == 0)
                throw new InvalidOperationException("No se pueden crear las imagenes: no se han proporcionado archivos.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var propertyFolder = Path.Combine(_environment.WebRootPath, "uploads", "properties", propertyId.ToString());

            // Crear el directorio si no existe
            if (!Directory.Exists(propertyFolder))
            {
                Directory.CreateDirectory(propertyFolder);
            }

            // Verificar si ya existe una imagen principal
            var hasMainImage = await _imageRepository.HasMainImageAsync(propertyId);

            var uploadedImages = new List<PropertyImage>();
            var isFirstImage = !hasMainImage;

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException($"El tipo de archivo {extension} no esta permitido.");

                var fileName = $"{Guid.NewGuid()}{extension}"; // Generar un nombre unico
                var physicalPath = Path.Combine(propertyFolder, fileName);

                // Guardar el archivo en el sistema de archivos
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Construir la URL accesible publicamente
                var url = $"{request.Scheme}://{request.Host}/uploads/properties/{propertyId}/{fileName}";

                uploadedImages.Add(new PropertyImage
                {
                    PropertyId = propertyId,
                    Url = url,
                    FileName = fileName,
                    IsMainImage = isFirstImage, // La primera imagen se marca como principal
                    CreationDate = DateTime.UtcNow
                });

                // Solo la primera imagen será principal
                if (isFirstImage)
                {
                    isFirstImage = false;
                }
            }

            await _imageRepository.AddRangeAsync(uploadedImages);
            var propertyImagesDTOs = uploadedImages.Select(pi => _mapper.Map<PropertyImageResponseDTO>(pi)).ToList();
            return propertyImagesDTOs;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var ownerId = _currentUserService.UserId;

            var image = await _imageRepository.GetByIdWithPropertyAsync(imageId);

            if (image == null)
                throw new ArgumentException("La imagen no existe.");

            // Validar que el usuario actual sea el propietario de la propiedad asociada a la imagen
            if (image.Property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta imagen.");

            var wasMainImage = image.IsMainImage;
            var propertyId = image.PropertyId;

            // Eliminar el archivo fisico si existe
            var physicalPath = Path.Combine(_environment.WebRootPath, "uploads", "properties", image.PropertyId.ToString(), image.FileName);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
            await _imageRepository.DeleteAsync(image);

            // Si era la imagen principal, marcar otra como principal
            if (wasMainImage)
            {
                var images = await _imageRepository.GetByPropertyIdAsync(propertyId);
                var nextImage = images.OrderBy(img => img.CreationDate).FirstOrDefault();

                if (nextImage != null)
                {
                    nextImage.IsMainImage = true;
                    await _imageRepository.UpdateAsync(nextImage);
                }
            }
        }

        public async Task<PropertyImageResponseDTO> SetMainImageAsync(int imageId)
        {
            var ownerId = _currentUserService.UserId;

            var newMain = await _imageRepository.GetByIdWithPropertyAsync(imageId);

            // Validar que la imagen exista
            if (newMain == null)
                throw new KeyNotFoundException("La imagen especificada no existe.");

            // Validar que el usuario actual sea el propietario de la propiedad asociada a la imagen
            if (newMain.Property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tienes permiso para modificar las imagenes de esta propiedad.");

            var images = await _imageRepository.GetByPropertyIdAsync(newMain.PropertyId);

            // Primero desactivar todas las imágenes principales
            foreach (var img in images)
            {
                img.IsMainImage = false;
                await _imageRepository.UpdateAsync(img);
            }

            // Luego activar la nueva imagen principal
            newMain.IsMainImage = true;
            await _imageRepository.UpdateAsync(newMain);

            return _mapper.Map<PropertyImageResponseDTO>(newMain);
        }

    }
}
