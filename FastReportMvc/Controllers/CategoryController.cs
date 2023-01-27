using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using FastReportMvc.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace FastReportMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly NorthwindContext _northwindContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CategoryController(IConfiguration configuration, NorthwindContext northwindContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this._configuration = configuration;
            this._northwindContext = northwindContext;
            this._hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("Northwind");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostingEnvironment.ContentRootPath, "reports", "Categories.frx"));
            webReport.ShowExports = true;
            var categories = GetTable<Category>(_northwindContext.Categories, "Categories");
            webReport.Report.RegisterData(categories, "Categories");
            return View(webReport);
        }


        [NonAction]
        public DataTable GetTable<TEntity>(IEnumerable<TEntity> table, string name) where TEntity : class
        {
            var offset = 78;
            DataTable result = new DataTable(name);
            PropertyInfo[] infos = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.PropertyType.IsGenericType
                && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    result.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                }
                else
                {
                    result.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            foreach (var el in table)
            {
                DataRow row = result.NewRow();
                foreach (PropertyInfo info in infos)
                {
                    if (info.PropertyType.IsGenericType &&
                        info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        object t = info.GetValue(el);
                        if (t == null)
                        {
                            t = Activator.CreateInstance(Nullable.GetUnderlyingType(info.PropertyType));
                        }

                        row[info.Name] = t;
                    }
                    else
                    {
                        if (info.PropertyType == typeof(byte[]))
                        {
                            //Fix for Image issue.
                            var imageData = (byte[])info.GetValue(el);
                            var bytes = new byte[imageData.Length - offset];
                            Array.Copy(imageData, offset, bytes, 0, bytes.Length);
                            row[info.Name] = bytes;
                        }
                        else
                        {
                            row[info.Name] = info.GetValue(el);
                        }
                    }
                }
                result.Rows.Add(row);
            }

            return result;
        }
        public IActionResult ExportReport()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("Northwind");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostingEnvironment.ContentRootPath, "reports", "categories.frx"));
            var filteredCategories = _northwindContext.Categories.Where(c => c.CategoryName.StartsWith("C"));
            var categories = GetTable<Category>(filteredCategories, "Categories");
            webReport.Report.RegisterData(categories, "Categories");
            webReport.Report.Prepare();
            Stream stream = new MemoryStream();
            webReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            // return stream in browser
            return File(stream, "application/zip", "report.pdf");
        }

    }
}
