


using System;
using Enova.ImportyKsiegowe.Interfaces;



namespace Soneta.GitHub.ExampleImportTxt
{
	/// <summary>
	/// Pomocniczy obiekt do komunikacji stanu odczytu pliku
	/// </summary>
	public class ExampleFormatReaderInfo : IReaderInfo
	{
		public string LineText { get; private set; }
		public int LineNumber { get; private set; }

		public ExampleFormatReaderInfo(int lineNumber, String lineText)
		{
			LineNumber = lineNumber;
			LineText = lineText;
			Info = String.Format("Wiersz {0} --> {1}", LineNumber, LineText);
		}

		public string Info { get; private set; }
		public ISourceInfo SourceInfo { get; set; }
	}
}



