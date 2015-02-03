


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Enova.ImportyKsiegowe;
using Enova.ImportyKsiegowe.ImportItems;
using Enova.ImportyKsiegowe.Interfaces;

using Soneta.EI;




namespace Soneta.GitHub.ExampleImportTxt
{
	/// <summary>
	/// Reader  - główny obiekt odpowiedzialny za interpretację danych wejścowych i przekształcanie ich obiekty ImportItem
	/// </summary>
	public class ExampleFormatReader : IImportReader
	{
		private readonly TextReader _reader;
		private int _lineNumber;


		public ISourceInfo SourceInfo { get; set; }



		/// <summary>
		/// Konstrukcja obiektu readera
		/// </summary>
		public ExampleFormatReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			// utworzenie strumienia wejściowego
			_reader = new StreamReader(stream, Encoding.UTF8);
		}


		/// <summary>
		/// Zwolnienie obiektu readera
		/// </summary>
		public void Dispose()
		{
			// zwolnienie strumienia wejściowego
			_reader.Dispose();
		}


		/// <summary>
		/// Metoda wymagana przez interfejs IImportReader
		/// (enumeruje obiekty ImportItem, które będą wprowadzane do bazy danych przez bibliotekę bazową)
		/// </summary>
		public IEnumerable<ImportItem> ReadAll()
		{
			ImportItem item;

			while ((item = Read()) != null)
				yield return item;
		}


		/// <summary>
		/// Metoda zwracająca kolejny obiekt ImportItem odczytany z pliku wejściowego
		/// (tu odbywa się interpretacja poszczególnych wierszy pliku w obsługiwanym formacie)
		/// </summary>
		public ImportItem Read()
		{
			string lineTxt;

			// odczytujemy kolejny wiersz wejściowy
			// (puste wiersze pomijamy)
			while ((lineTxt = _reader.ReadLine()) == String.Empty)
				_lineNumber++;

			// wczytanie null oznacza koniec strumienia -> koniec importu
			if (lineTxt == null)
				return null;

			// tworzymy obiekt informacyjny o pozycji w pliku
			// (numery wierszy zapewniają możliwość sygnalizacji miejsca wystąpienia błędów)
			var info = new ExampleFormatReaderInfo(++_lineNumber, lineTxt) { SourceInfo = SourceInfo };

			try
			{
				// comma reader - klasa pomocnicza do czytania formatu CSV
				var commaReader = new CommaReaderPlus(lineTxt, ';');

				//
				// przekształcamy wiersze wejściowe na obiekty pośrednie
				// (jeden wiersz = jeden obiekt)
				//

				// na pierwszej pozycji mamy zakodowany rodzaj obiektu wg przyjętej przez nas konwencji
				var type = commaReader.ReadString();

				switch (type)
				{
					case "ESP":
						return ReadEsp(commaReader, info);
					case "RAP":
						return ReadRaportEsp(commaReader, info) ;
					case "KON":
						return  ReadKontrahent(commaReader, info) ;
					case "WPL":
						return  ReadZaplata(commaReader, new Wplata { ReaderInfo = info }) ;
					case "WYP":
						return  ReadZaplata(commaReader, new Wyplata { ReaderInfo = info }) ;
				}

				throw new Exception(string.Format("Nieznany typ wiersza {0}", type));
			}
			catch (Exception ex)
			{
				var msg = string.Format("ExampleFormatReader - błąd wczytywania: {0}", info.Info);
				throw new Exception(msg, ex);
			}

		}


		/// <summary>
		/// Utworzenie obiektu "EwidencjaSrodkowPienieznych"
		/// (wskazuje symbol ewidencji, do której zostaną skierowane raportu)
		/// </summary>
		private static EwidencjaSrodkowPienieznych ReadEsp(CommaReader commaReader, ExampleFormatReaderInfo info)
		{
			return new EwidencjaSrodkowPienieznych
			{
				ReaderInfo = info,
				Symbol = commaReader.ReadString()
			};
		}


		/// <summary>
		/// Utworzenie obiektu "RaportEsp"
		/// (w enova powstanie obiekt raportu)
		/// </summary>
		private static RaportEsp ReadRaportEsp(CommaReader commaReader, ExampleFormatReaderInfo info)
		{
			return new RaportEsp
			{
				ReaderInfo = info,
				Definicja = commaReader.ReadString(),
				Data = commaReader.ReadDate(),
				Numer = commaReader.ReadString()
			};
		}


		/// <summary>
		/// Utworzenie obiektu "Kontrahent"
		/// (w enova będą tworzeni lub aktualizowani kontrahenci)
		/// </summary>
		private static Kontrahent ReadKontrahent(CommaReader commaReader, IReaderInfo info)
		{
			return new Kontrahent
			{
				ReaderInfo = info,
				Kod = commaReader.ReadString(),
				Nazwa = commaReader.ReadString(),
				Nip = String.Empty
			};
		}


		/// <summary>
		/// Uzupełnienie właściwości obiektu "Zaplata"
		/// (w enova powstaną wpłaty i wypłaty w założonym wcześniej raporcie ESP)
		/// </summary>
		private static Zaplata ReadZaplata(CommaReader commaReader, Zaplata zaplata)
		{
			zaplata.Numer = String.Empty;
			zaplata.Data = commaReader.ReadDate();
			zaplata.PodmiotKod = commaReader.ReadString();
			zaplata.SposobPlatnosci = commaReader.ReadString();
			zaplata.Kwota = commaReader.ReadDecimal();
			zaplata.KwotaWaluta = commaReader.ReadString();
			zaplata.ZaplataZa = commaReader.ReadString();
			zaplata.Opis = commaReader.ReadString();

			return zaplata;
		}
	}
}