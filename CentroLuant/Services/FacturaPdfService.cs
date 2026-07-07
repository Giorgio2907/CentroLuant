using CentroLuant.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CentroLuant.Services
{
    public class FacturaPdfService
    {
        public byte[] GenerarPdf(Factura factura)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("CENTRO ODONTOLÓGICO LUANT")
                            .FontSize(20).Bold().AlignCenter();
                        col.Item().Text("Atención integral odontológica")
                            .FontSize(11).AlignCenter();
                        col.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().PaddingBottom(5).Row(row =>
                        {
                            row.RelativeItem().Text($"FACTURA N° {factura.ID_Factura}").Bold().FontSize(14);
                            row.RelativeItem().AlignRight().Text($"Fecha: {factura.FechaEmision}");
                        });

                        col.Item().PaddingBottom(5).Text($"Estado: {factura.EstadoPago}").Bold();

                        col.Item().PaddingTop(10).Text("DATOS DEL PACIENTE").Bold().Underline();
                        col.Item().Text($"Paciente: {factura.NombrePaciente}");
                        col.Item().Text($"DNI: {factura.DNI_Paciente}");

                        col.Item().PaddingTop(10).Text("SERVICIOS FACTURADOS").Bold().Underline();
                        col.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#1a73e8").Padding(5)
                                    .Text("Descripción").FontColor("#ffffff").Bold();
                                header.Cell().Background("#1a73e8").Padding(5)
                                    .Text("Monto").FontColor("#ffffff").Bold().AlignRight();
                            });

                            table.Cell().Padding(5).Text(factura.DescripcionServicios ?? "");
                            table.Cell().Padding(5).AlignRight().Text($"S/ {factura.MontoTotal}");
                        });

                        col.Item().PaddingTop(15).AlignRight()
                            .Text($"TOTAL: S/ {factura.MontoTotal}").Bold().FontSize(14);

                        col.Item().PaddingTop(30).LineHorizontal(1);
                        col.Item().PaddingTop(5).Text("Gracias por confiar en Centro Odontológico Luant")
                            .AlignCenter().FontSize(10);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}