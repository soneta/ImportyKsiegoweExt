



using Enova.ImportyKsiegowe;
using Soneta.Business;
using Soneta.Business.ImpExp;
using Soneta.GitHub.ExampleImportTxt;



//
// rejesteracja filtra importu pod określoną nazwą
// filtr będzie dostępny w enova w PLIK\IMPORTUJ ZAPISY
//
[assembly: ImportFilter("IMPORT TXT PRZYKŁAD Z GITHUB...", typeof(ExampleFormatFilter))]


namespace Soneta.GitHub.ExampleImportTxt
{
	public class ExampleFormatFilter : ImportFilter
	{
		/// <summary>
		/// Główna metoda import - wywoływana przez enova do przeprowadzenia wszystkich operacji
		/// </summary>
		public override object CreateReader10()
		{
			// żądanie odpytania o plik importowy i przekazanie sterowania do metody "Import" po wybraniu pliku
			return QueryContextInformation.Create<NamedStream>(Import);
		}


		private object Import(NamedStream namedStream)
		{
			// utworzenie obiektu importera z domyślnymi parametami
			var importer = new ExampleFormatImporter(new ImportParams(Login, null));

			// przeprowadzenie importu
			var importResult = importer.Import(namedStream);

			// wyświetlenie końcowego wyniku operacji
			return ImportMessage.GetMessageBoxInformation(importResult);
		}
	}
}