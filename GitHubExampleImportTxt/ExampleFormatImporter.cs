


using System.IO;
using Enova.ImportyKsiegowe;
using Enova.ImportyKsiegowe.Interfaces;




namespace Soneta.GitHub.ExampleImportTxt
{
	/// <summary>
	/// Klasa importera
	/// (w tym wypadku dziedziczymy z klasy FileImporter z Enova.ImportyKsiegowe co zapewnia standardową obsługę importu z pliku)
	/// </summary>
	public class ExampleFormatImporter : FileImporter
	{
		public ExampleFormatImporter(ImportParams @params)
			: base(@params)
		{ }

		/// <summary>
		/// Metoda tworząca reader - główny obiekt odpowiedzialny za import
		/// </summary>
		protected override IImportReader CreateReader(Stream stream, ISourceInfo sourceInfo)
		{ return new ExampleFormatReader(stream) { SourceInfo = sourceInfo }; }
	}
}



