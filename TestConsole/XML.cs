using System.Xml.Linq;
using System.Xml.XPath;

public class XML
{
	const string Quantity = "Quantity";
	const string Tzr = "Tzr";
	const string Tzm = "Tzm";
	const string Mch = "Mch";
	const string Mat = "Mat";
	const string WorkList = "WorkList";

	public List<BaseResources> GetBase(XElement elPosition, string xpath, string teg)
	{
		XElement[] els = elPosition.XPathSelectElements(xpath)?.ToArray();

		if (els == null || els.Count() <= 0) return default;

		int sizeArr = els.Count();
		List<BaseResources> baseResources = new List<BaseResources>();



		BaseResources baseRes = null;

		switch (teg)
		{
			case Tzr: baseRes = new Tzr(); break;
			case Tzm: baseRes = new Tzm(); break;
			case Mch: baseRes = new Mch(); break;
			case Mat: baseRes = new Mat(); break;
		}


		for (int i = 0; i < sizeArr; i++)
		{
			XElement el = els[i];
			var dicAtr = GetDicAtributes(el);


			baseRes.Caption = GetValueAttribute(dicAtr, "Caption");
			baseRes.Code = GetValueAttribute(dicAtr, "Code");
			baseRes.Units = GetValueAttribute(dicAtr, "Units");
			baseRes.Quantity = GetValueAttribute(dicAtr, "Quantity");
			baseRes.WorkClass = GetValueAttribute(dicAtr, "WorkClass");


			XElement elPrice = el.XPathSelectElement(".//PriceCurr");
			var dicAtrPrices = GetDicAtributes(elPrice);
			PriceCurr price = new PriceCurr()
			{
				Value = GetValueAttribute(dicAtrPrices, "Value"),
				Comment = GetValueAttribute(dicAtrPrices, "Comment"),
				ZM = GetValueAttribute(dicAtrPrices, "ZM"),
			};

			baseRes.PriceCurr = price;
			baseResources.Add(baseRes);

		}
		return baseResources;
	}




	public List<Positions> GetXmlElements(string pathFileXml)
	{

		XElement root = XElement.Load(pathFileXml);
		//получаем все позиции
		XElement[] rootElements = root.Descendants("Position").ToArray();

		if (rootElements == null || rootElements.Length <= 0) return default;


		List<Positions> positions = new List<Positions>();

		//перебор POSITION
		for (int i = 0; i < rootElements.Length; i++)
		{

			XElement elPosition = rootElements[i];

			Positions position = new Positions();

			List<Quantity> quantitys = new List<Quantity>();
			List<WorksList> worksLists = new List<WorksList>();


			//получаем все атрибуты Position
			var dicAtrPosition = GetDicAtributes(elPosition);
			//Console.WriteLine(dicAtrPosition);
			//Console.WriteLine($"\n***********");
			//Console.WriteLine(elPosition.Name);

			//Атрибуты
			position.Caption = GetValueAttribute(dicAtrPosition, "Caption");
			position.Number = GetValueAttribute(dicAtrPosition, "Number");
			position.Code = GetValueAttribute(dicAtrPosition, "Code");
			position.Units = GetValueAttribute(dicAtrPosition, "Units");
			position.SysID = GetValueAttribute(dicAtrPosition, "SysID");
			position.PriceLevel = GetValueAttribute(dicAtrPosition, "PriceLevel");
			position.Quantity = GetValueAttribute(dicAtrPosition, "Quantity");
			position.Options = GetValueAttribute(dicAtrPosition, "Options");
			position.PzSync = GetValueAttribute(dicAtrPosition, "PzSync");
			position.Vr2001 = GetValueAttribute(dicAtrPosition, "Vr2001");
			position.Mass = GetValueAttribute(dicAtrPosition, "Mass");



			position.Resources = GetResources(elPosition);


			positions.Add(position);

		}


		return positions;


	}


	Resources GetResources(XElement elPosition)
	{
		Resources resources = new Resources();

		List<BaseResources> tzrs = GetBase(elPosition, ".//Tzr", Tzr);
		List<BaseResources> tzms = GetBase(elPosition, ".//Tzm", Tzm);
		List<BaseResources> mchs = GetBase(elPosition, ".//Mch", Mch);
		List<BaseResources> mats = GetBase(elPosition, ".//Mat", Mat);

		resources.TZR = tzms != null ? tzrs.Select(r => (Tzr)r)?.ToArray() : null;
		resources.TZM = tzms != null ? tzms.Select(r => (Tzm)r)?.ToArray() : null;
		resources.MCH = mchs != null ? mchs.Select(r => (Mch)r)?.ToArray() : null;
		resources.MAT = mats != null ? mats.Select(r => (Mat)r)?.ToArray() : null;


		//Console.WriteLine(resources);
		return resources;

	}


	public static Dictionary<string, List<string>> GetDicAtributes(XElement el)
	{
		if (el == null) return default;
		Dictionary<string, List<string>> dicAttributes = new Dictionary<string, List<string>>();
		foreach (XAttribute atr in el?.Attributes())
		{
			Console.WriteLine($"----> {atr.Name} = {atr?.Value} ");
			try
			{
				dicAttributes.AddDic(atr.Name.ToString(), atr?.Value);
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }

		}

		return dicAttributes;
	}

	public static string GetValueAttribute(Dictionary<string, List<string>> dicAttributes, string nameAttribute)
	{
		if (dicAttributes == null || dicAttributes.Count <= 0) return default;

		if (dicAttributes.ContainsKey(nameAttribute))
		{
			string result = String.Join("\n", dicAttributes[nameAttribute]);
			return result;
		}
		else return default;

	}
}






