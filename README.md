# FastReportMvc

- https://www.syncfusion.com/
- DevExpress ve syncfusion reporting raporlama işlerinizde de kullanabileceğimiz alternatif toollardır.

# Project 1 - FastReportMvc

- FastReport.Community -> sadece reporting (rapor) işlerinde işimize yarıyor bu tool
- Çalıştırıp dosyayo frm olarak olustur. FastReport.Community -> Designer çalıştır olusur.
- MVC içinde wwwroot içine Reports klasörü açıp kopyala

- Aşağıdaki paketler install edildi
  - FastReport.OpenSource
  - FastReport.OpenSource.Data.MsSql
  - FastReport.OpenSource.Web
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCoreDesign
  - Microsoft.EntityFrameworkCore.SqlServer
  - FastReport.OpenSource.Export.PdfSimple

- Northwind databasei scaffold edildi. `dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=Northwind;Trusted_Connection=True" Microsoft.EntityFrameWorkCore.SqlServer -o Entities`

- Controller 
  - CustomerController
  - CategoryController
