using Application.DTOs.Payments;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    // Mapeos de Payment hacia los DTOs de respuesta simple y listado de pagos pendientes.
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentResponseDTO>()
            .ForMember(dest => dest.paymentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status.ToString()));


            CreateMap<Payment, PendingPaymentListDTO>()
           .ForMember(
               dest => dest.PaymentId,
               opt => opt.MapFrom(src => src.Id)
           )
           .ForMember(
                dest => dest.ReservationId,
                opt => opt.MapFrom(src => src.Reservation.Id)
           )
           .ForMember(
               dest => dest.PropertyName,
               opt => opt.MapFrom(src => src.Reservation.Property.Title)
           )
           .ForMember(
               dest => dest.GuestEmail,
               opt => opt.MapFrom(src => src.Reservation.Guest.Email)
           )
           .ForMember(
               dest => dest.ReservationStart,
               opt => opt.MapFrom(src => src.Reservation.StartDate)
           )
           .ForMember(
               dest => dest.ReservationEnd,
               opt => opt.MapFrom(src => src.Reservation.EndDate)
           )
           .ForMember(
               dest => dest.Amount,
               opt => opt.MapFrom(src => src.Reservation.TotalPrice)
           )
           .ForMember(
               dest => dest.PaymentMethod,
               opt => opt.MapFrom(src => src.Method.ToString())
           )
           .ForMember(
               dest => dest.Status,
               opt => opt.MapFrom(src => src.Status.ToString())
           )
           .ForMember(
               dest => dest.UploadedAt,
               opt => opt.MapFrom(src => src.PaymentDate)
           );
        }
    }
}
