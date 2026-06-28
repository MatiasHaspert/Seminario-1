namespace WinFormsApp
{
    /// <summary>
    /// Paleta y helpers de estilo compartidos por todos los formularios del panel de Administración.
    /// Para no romper el diseñador, los helpers se invocan desde el code-behind (después de
    /// InitializeComponent) — los .Designer.cs usan colores literales equivalentes.
    /// </summary>
    internal static class UiTheme
    {
        // ── Paleta ────────────────────────────────────────────────────────────
        public static readonly Color HeaderBg = Color.FromArgb(44, 62, 80);   // Slate
        public static readonly Color HeaderText = Color.White;
        public static readonly Color HeaderSubtle = Color.FromArgb(189, 195, 199);

        public static readonly Color FormBg = Color.White;
        public static readonly Color SurfaceAlt = Color.FromArgb(236, 240, 241); // Status bar / sub-panels

        public static readonly Color TextPrimary = Color.FromArgb(52, 73, 94);
        public static readonly Color TextMuted = Color.FromArgb(127, 140, 141);

        public static readonly Color Primary = Color.FromArgb(41, 128, 185);
        public static readonly Color PrimaryHover = Color.FromArgb(52, 152, 219);
        public static readonly Color Success = Color.FromArgb(39, 174, 96);
        public static readonly Color Warning = Color.FromArgb(243, 156, 18);
        public static readonly Color Danger = Color.FromArgb(192, 57, 43);
        public static readonly Color Border = Color.FromArgb(189, 195, 199);

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>Aplica estilo "plano + paleta" sobre un DataGridView.</summary>
        public static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = FormBg;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(230, 230, 230);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = HeaderText;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = HeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            dgv.ColumnHeadersHeight = 32;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 234, 248);
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            dgv.RowTemplate.Height = 28;
        }
    }
}
