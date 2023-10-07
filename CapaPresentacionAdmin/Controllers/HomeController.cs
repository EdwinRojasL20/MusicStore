using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CapaPresentacionAdmin.Controllers
{

    [Authorize]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<USUARIO> oLista = new List<USUARIO>();
            oLista = new CN_Usuarios().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(USUARIO objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0) 
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id,out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {

            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().VerDashBoard();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("en-CO");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in oLista)
            {
                dt.Rows.Add(new object[] { 
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";


            using (XLWorkbook wb = new XLWorkbook())
            {

                var worksheet = wb.Worksheets.Add("MiHojaDeDatos"); // Reemplaza "MiHojaDeDatos" con el nombre que desees
                worksheet.Cell(1, 1).InsertTable(dt); // Inserta la tabla de datos en la hoja de cálculo
                using (MemoryStream stream = new MemoryStream())

                //    wb.Worksheet.Add(dt);
                //using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }


        [HttpPost]
        public FileResult ExportarVentaPDF(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            // Crear un documento PDF
            Document doc = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            doc.Open();

            // Agregar contenido al PDF
            PdfPTable table = new PdfPTable(7); // 7 columnas
            table.WidthPercentage = 100;
            float[] widths = new float[] { 20f, 20f, 20f, 10f, 10f, 10f, 20f };
            table.SetWidths(widths);

            // Agregar encabezados
            table.AddCell(new PdfPCell(new Phrase("Fecha Venta")));
            table.AddCell(new PdfPCell(new Phrase("Cliente")));
            table.AddCell(new PdfPCell(new Phrase("Producto")));
            table.AddCell(new PdfPCell(new Phrase("Precio")));
            table.AddCell(new PdfPCell(new Phrase("Cantidad")));
            table.AddCell(new PdfPCell(new Phrase("Total")));
            table.AddCell(new PdfPCell(new Phrase("Id Transaccion")));

            // Agregar filas de datos
            foreach (Reporte rp in oLista)
            {
                table.AddCell(new PdfPCell(new Phrase(rp.FechaVenta)));
                table.AddCell(new PdfPCell(new Phrase(rp.Cliente)));
                table.AddCell(new PdfPCell(new Phrase(rp.Producto)));
                table.AddCell(new PdfPCell(new Phrase(rp.Precio.ToString())));
                table.AddCell(new PdfPCell(new Phrase(rp.Cantidad.ToString())));
                table.AddCell(new PdfPCell(new Phrase(rp.Total.ToString())));
                table.AddCell(new PdfPCell(new Phrase(rp.IdTransaccion)));
            }

            doc.Add(table);

            doc.Close();

            // Devolver el PDF como un archivo descargable
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=ReporteVenta.pdf");
            Response.Buffer = true;
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();

            byte[] pdfBytes = memoryStream.ToArray();
            return File(pdfBytes, "application/pdf", "ReporteVenta.pdf");

        }

    }
}