using PropertyGridEx;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using XmlSerialCtrl;

namespace PropertyGridTest
{
	public class SampleClass : Serial<SampleClass>
	{

		#region プロパティ
		
		/// <summary>
		/// ファイルを開くダイアログエディタのサンプル
		/// </summary>
		[XmlElement("ファイル")]
		[Description("あるファイルのパス"),
		Category("エディタサンプル"), DisplayName("ファイル"),
		 Editor(typeof(FileOpenEditor), typeof(UITypeEditor)),
		 FileOpen("テキスト(*.txt,*.c*,*.vb*,*.ini)|*.txt;*.c*;*.vb*;*.ini|エクセルブック(*.xls*)|*.xls*|画像(*.bmp,*.png,*.jpg)|*.bmp;*.png;*.jpg|すべてのファイル(*.*)|*.*")
		 //FileOpen( "画像 | *.bmp;*.png;*.jpg | すべてのファイル( *.*) | *.*")
		]
		public string File { get; set; }

		/// <summary>
		/// フォルダを開くダイアログエディタのサンプル
		/// </summary>
		[XmlElement("フォルダ")]
		[Description("あるフォルダのパス"),
		Category("エディタサンプル"), DisplayName("フォルダ"),
		 Editor(typeof(FolderOpenEditor), typeof(UITypeEditor))
		]
		public string DirPath { get; set; }
		
		/// <summary>
		/// 艦種
		/// </summary>
		[XmlElement( "艦種" )]
		[Description( "艦船の種類" ),
		 Category( "コンボボックスサンプル" ), DisplayName( "艦種" )]
		[TypeConverter( typeof( StringArrayConverter ) ), StringArray( 0 )]
		public string CategoryName { get; set; }
		/// <summary>
		/// 型名
		/// </summary>
		[XmlElement( "型名" )]
		[Description( "艦船のグループ名" ),
		 Category( "コンボボックスサンプル" ), DisplayName( "型名" )]
		[TypeConverter( typeof( StringArrayConverter ) ), StringArray( 1 )]
		public string ClassName { get; set; }

		/// <summary>
		/// インターフェイスコレクション
		/// </summary>
		[XmlElement("コレクションアイテム")]
		[Description( "インターフェイス[ISampleIF] のコレクション" ),
		 Category( "コレクションエディタサンプル" ), DisplayName( "複数アイテム" )]
		[Editor(typeof(InterfaceCollectionEditor),typeof(UITypeEditor))]
		public SerlList<ISampleIF> TestList { get; set; }
		
		/// <summary>
		/// ダイアログ選択用豆腐の種類
		/// </summary>
		[XmlElement( "豆腐" )]
		[Description( "豆腐の種類(ダイアログ選択)" ) ,
		 Category("フォーム・コントロール"), DisplayName("豆腐(ダイアログ)")]
		[Editor( typeof( UserFormEditor ), typeof( UITypeEditor ) ),
		 UserFormAttribute( 0 )]
		public int TofuType { get; set; }

		/// <summary>
		/// プルダウン選択用豆腐の種類
		/// </summary>
		[XmlElement("豆腐その2")]
		[Description("豆腐の種類(プルダウン選択)"),
		 Category( "フォーム・コントロール" ), DisplayName( "豆腐(プルダウン)")]
		[Editor(typeof( UserCtrlEditor ),typeof(UITypeEditor)),
		 UserCtrlAttribute(0)]
		public int TofuType2 { get; set; }

		#endregion

		#region コンストラクタ

		public SampleClass()
		{
			File = "";
			DirPath = "";
			TestList = new SerlList<ISampleIF>( );
			CategoryName = "駆逐艦";
			ClassName = "暁型";
			TofuType = 1;
			TofuType2 = 2;
		}
		
		#endregion

	}

	// インターフェイス
	public interface ISampleIF
	{
		string Name { get; set; }
	}

	// ISampleIFを実装したクラス１
	[XmlRoot("サンプルクラス1")]
	[XmlType( "SampleSub1" )]
	public class SampleSub1 : ISampleIF
	{
		[XmlAttribute("名前")]
		[Category("サンプル１"),DisplayName("名前")]
		public string Name { get; set; }

		public SampleSub1( )
		{
			Name = "";
		}

		public override string ToString( )
		{
			return Name;
		}
	}

	// ISampleIFを実装したクラス２
	[XmlRoot( "サンプルクラス2" )]
	[XmlType( "SampleSub2" )]
	public class SampleSub2 : ISampleIF
	{
		[XmlAttribute( "名前" )]
		[Category( "サンプル２" ), DisplayName( "名前" )]
		public string Name { get; set; }

		[XmlAttribute( "値" )]
		[Category( "サンプル２" ), DisplayName( "値" )]
		public int Value { get; set; }

		public SampleSub2( )
		{
			Name = "";
			Value = 0;
		}


		public override string ToString( )
		{
			return Name;
		}
	}

}
