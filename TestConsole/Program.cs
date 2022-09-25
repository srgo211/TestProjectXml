using System.Text;

Console.WriteLine("Hello, World!");

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding.GetEncoding("windows-1254");

string path = @"D:\VisualStudio\XmlWPF\ReplaseCore\Эталон 14.xml";

XML xml = new XML();
var x = xml.GetXmlElements(path);


Console.WriteLine(x);

#region Models
public class Positions
{

	public string Caption { get; set; }
	public string Number { get; set; }
	public string Code { get; set; }
	public string Units { get; set; }
	public string SysID { get; set; }
	public string PriceLevel { get; set; }
	public string Quantity { get; set; }
	public string Options { get; set; }
	public string PzSync { get; set; }
	public string Vr2001 { get; set; }
	public string Mass { get; set; }

	public Quantity Quantitys { get; set; }
	public Resources Resources { get; set; }
	public Koefficient Koefficient { get; set; }
	public List<WorksList> Works { get; set; }

}


public class Quantity
{
	public int Fx { get; set; }
	public int Result { get; set; }
}

#region Resources
public class Resources
{
	public Tzr[] TZR { get; set; }
	public Tzm[] TZM { get; set; }
	public Mch[] MCH { get; set; }
	public Mat[] MAT { get; set; }
}

public class Tzr : BaseResources, IBaseResources { }

public class Tzm : BaseResources, IBaseResources { }

public class Mch : BaseResources, IBaseResources { }

public class Mat : BaseResources, IBaseResources
{
	public string Options { get; set; }
	public string Mass { get; set; }
	public List<Koefficient> Koefficients { get; set; }
}
#endregion

public class PriceCurr
{
	public string Value { get; set; }

	public string Comment { get; set; }

	public string ZM { get; set; }
}

public class Koefficient
{
	public string Caption { get; set; }
	public string Options { get; set; }
	public int Level { get; set; }
}

public class WorksList
{
	public string Caption { get; set; }
}

public class CustomNPCurr
{
	public string Nacl { get; set; }
	public string NaclMask { get; set; }
	public string Plan { get; set; }
	public string PlanMask { get; set; }
}


public abstract class BaseResources
{
	public string Caption { get; set; }

	public string Code { get; set; }

	public string Units { get; set; }

	public string Quantity { get; set; }

	public string WorkClass { get; set; }

	public PriceCurr PriceCurr { get; set; }
}


public interface IBaseResources
{
	string Caption { get; set; }

	string Code { get; set; }

	string Units { get; set; }

	string Quantity { get; set; }

	string WorkClass { get; set; }

	PriceCurr PriceCurr { get; set; }
}

#endregion


#region  Расширение
public static class Expansion
{
	public static string PathFileXML { get; set; }
	public static void AddDic(this Dictionary<string, List<string>> dic, string key, string value)
	{
		if (dic == null) return;

		if (dic.ContainsKey(key))
		{
			var res = dic[key];
			res.Add(value);
		}
		else
		{
			List<string> newList = new List<string>();
			newList.Add(value);
			dic.Add(key, newList);

		}
	}


	private static string GetValueAttribute(Dictionary<string, List<string>> dicAttributes, string nameAttribute)
	{
		if (dicAttributes.ContainsKey(nameAttribute))
		{
			string result = String.Join("\n", dicAttributes[nameAttribute]);
			return result;
		}
		else return default;

	}
}
#endregion





