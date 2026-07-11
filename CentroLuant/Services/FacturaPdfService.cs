using CentroLuant.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CentroLuant.Services
{
    public class FacturaPdfService
    {
        // Paleta acorde al sistema
        private const string AzulOscuro = "#0a2d6e";
        private const string Azul = "#1565c0";
        private const string Teal = "#0097a7";
        private const string Naranja = "#e65100";
        private const string NaranjaClaro = "#ef6c00";
        private const string GrisTexto = "#333333";
        private const string GrisSuave = "#888888";
        private const string FondoSuave = "#f8faff";
        private const string BordeSuave = "#e8f0fe";

        public byte[] GenerarPdf(Factura factura)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontSize(10).FontColor(GrisTexto));

                    page.Header().Element(c => ComponerHeader(c, factura));

                    page.Content().PaddingHorizontal(35).PaddingTop(25).Column(col =>
                    {
                        col.Spacing(18);

                        col.Item().Element(c => ComponerTituloFactura(c, factura));
                        col.Item().Element(c => ComponerDatosPaciente(c, factura));
                        col.Item().Element(c => ComponerTablaServicios(c, factura));
                        col.Item().Element(c => ComponerTotal(c, factura));
                    });

                    page.Footer().Element(ComponerFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComponerHeader(IContainer container, Factura factura)
        {
            container.Background(AzulOscuro).Height(100).Row(row =>
            {
                row.RelativeItem().PaddingLeft(35).PaddingVertical(20).Column(col =>
                {
                    col.Item().Text("CENTRO ODONTOLÓGICO LUANT")
                        .FontSize(19).Bold().FontColor(Colors.White);
                    col.Item().PaddingTop(3).Text("Atención integral odontológica")
                        .FontSize(9.5f).FontColor("#cfe0fb");
                });

                row.ConstantItem(140).AlignMiddle().PaddingRight(35).Column(col =>
                {
                    col.Item().AlignRight().Text("FACTURA")
                        .FontSize(12).Bold().FontColor(Colors.White);
                    col.Item().AlignRight().PaddingTop(2)
                        .Text($"N° {factura.ID_Factura:D5}").FontSize(9).FontColor("#cfe0fb");
                });
            });
        }

        private void ComponerTituloFactura(IContainer container, Factura factura)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text($"Factura N° {factura.ID_Factura:D5}")
                        .FontSize(15).Bold().FontColor(AzulOscuro);
                    col.Item().PaddingTop(2).Text($"Fecha de emisión: {factura.FechaEmision}")
                        .FontSize(9.5f).FontColor(GrisSuave);
                });

                row.ConstantItem(130).AlignRight().AlignMiddle()
                    .Background(EstadoColorFondo(factura.EstadoPago))
                    .PaddingVertical(6).PaddingHorizontal(12)
                    .AlignCenter()
                    .Text(factura.EstadoPago ?? "Pendiente")
                    .FontSize(9.5f).Bold().FontColor(EstadoColorTexto(factura.EstadoPago));
            });
        }

        private void ComponerDatosPaciente(IContainer container, Factura factura)
        {
            container.Background(FondoSuave).Border(1).BorderColor(BordeSuave)
                .Padding(14).Column(col =>
                {
                    col.Item().Text("DATOS DEL PACIENTE")
                        .FontSize(9).Bold().FontColor(Teal);

                    col.Item().PaddingTop(8).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Paciente").FontSize(8).FontColor(GrisSuave);
                            c.Item().PaddingTop(2).Text(factura.NombrePaciente ?? "-")
                                .FontSize(10.5f).Bold().FontColor(AzulOscuro);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("DNI").FontSize(8).FontColor(GrisSuave);
                            c.Item().PaddingTop(2).Text(factura.DNI_Paciente ?? "-")
                                .FontSize(10.5f).Bold().FontColor(AzulOscuro);
                        });
                    });
                });
        }

        private void ComponerTablaServicios(IContainer container, Factura factura)
        {
            container.Column(col =>
            {
                col.Item().Text("SERVICIOS FACTURADOS")
                    .FontSize(9).Bold().FontColor(Teal);

                col.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(3);
                        cols.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(AzulOscuro).Padding(8)
                            .Text("Descripción").FontColor(Colors.White).Bold().FontSize(9.5f);
                        header.Cell().Background(AzulOscuro).Padding(8)
                            .AlignRight().Text("Monto").FontColor(Colors.White).Bold().FontSize(9.5f);
                    });

                    table.Cell().Background(Colors.White).BorderBottom(1).BorderColor(BordeSuave)
                        .Padding(8).Text(factura.DescripcionServicios ?? "");
                    table.Cell().Background(Colors.White).BorderBottom(1).BorderColor(BordeSuave)
                        .Padding(8).AlignRight().Text($"S/ {factura.MontoTotal:0.00}").FontColor(AzulOscuro).Bold();
                });
            });
        }

        private void ComponerTotal(IContainer container, Factura factura)
        {
            container.AlignRight().Background(Naranja).PaddingVertical(10).PaddingHorizontal(20)
                .Width(220).Row(row =>
                {
                    row.RelativeItem().Text("TOTAL A PAGAR").FontColor(Colors.White).Bold().FontSize(11);
                    row.ConstantItem(90).AlignRight()
                        .Text($"S/ {factura.MontoTotal:0.00}").FontColor(Colors.White).Bold().FontSize(13);
                });
        }

        private void ComponerFooter(IContainer container)
        {
            container.BorderTop(1).BorderColor(BordeSuave).PaddingTop(10).PaddingBottom(15)
                .PaddingHorizontal(35).Column(col =>
                {
                    col.Item().AlignCenter().Text("Gracias por confiar en Centro Odontológico Luant")
                        .FontSize(9.5f).FontColor(AzulOscuro).Bold();
                    col.Item().PaddingTop(2).AlignCenter()
                        .Text("Av. Universitaria 1801, Lima, Perú · +51 987 654 321 · centroluant2026@gmail.com")
                        .FontSize(8).FontColor(GrisSuave);
                });
        }

        private string EstadoColorFondo(string? estado) => estado switch
        {
            "Pagada" => "#e8f5e9",
            "Pendiente" => "#fff8e1",
            "Anulada" => "#ffebee",
            _ => "#f0f4f8"
        };

        private string EstadoColorTexto(string? estado) => estado switch
        {
            "Pagada" => "#2e7d32",
            "Pendiente" => "#f57f17",
            "Anulada" => "#c62828",
            _ => "#555555"
        };
    }
}