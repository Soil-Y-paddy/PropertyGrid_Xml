# <a id="c2">XMLシリアライズ・デシリアライズ</a>

* 目次
    * [ 基本的な使用方法 ](#s201)
    * [ 属性 ](#s202)
    * [ `Serial<T>` シリアライズ基底クラス ](#s21)
    * [ `SerlList<T>` シリアライズ可能な汎用リスト ](#s22)
    * [ `SerlDic<T>` シリアライズ可能な汎用辞書 ](#s23)

<dl>
<dt>名前空間</dt><dd>XmlSerialCtrl</dd>
<dt>パス</dt><dd>Lib/XmlSerialCtrl/</dd>
</dl>

## <a id="s201"> 基本的な使用方法</a>
1. 設定を取りまとめたクラスを作成する
    * publicなクラスにする
    * 引数なしのコンストラクタを必ず実装する
    * 設定値は、publicなプロパティで設定する。
```csharp
public class Settings
{
	public string DbPath{get;set;}
	
	public Settings()
	{
		DbPath = "DefaultValue";
	}
}
```

2. XmlSerialCtrl.csをプロジェクトに取り込み、1.で作ったクラスの上部にusingする
```csharp
using XmlSerialCtrl;
```
3.  1.で作ったクラスに、`Serial<T>`を継承する。
`T`は、自クラスを指定する
```csharp
public class Settings:Serial<Settings> // テンプレートは自クラスを指定
{
	//... 中略
}
```
4. ファイルの読込
```csharp
strSample1= "sampleSetting.xml"
Settings objSmpl = Settings.Load(strSample1);
```
5. ファイルの書込
```csharp
objSmpl.Save(strSample1);
```
6. 上記例のファイル内容
```XML
<?xml version="1.0" encoding="utf-8"?>
<Settings>
	<DbPath>DefaultValue</DbPath>
</Settings>
```
* ファイル名は、実装クラスに内包しても良い
```csharp

public class Settings:Serial<Settings>
{
	public const string SETTING_FILE = "sampleSettings.xml";

	//... 中略

	public void Save()
	{
		Save(SETTING_FILE);
	}
	public static Settings Load()
	{
		return Load(SETTING_FILE);
	}
}
```

## <a id="s202"> 属性 </a>

1. XMLに出力しないプロパティは、XmlIgnore()属性を設定する。

```csharp
using System.Xml.Serialization;
	public class Settings: Serial<Settings>
	{

		// 出力しない
		[XmlIgnore()]
		public string Hidden { get; set; }
		
		public Settings()
		{
		}

	}
```
2. 要素名を変更する

```csharp
using System.Xml.Serialization;
	public class Settings: Serial<Settings>
	{
		// 要素名の変更
		[XmlElement("SampleData")]
		public string Sample { get; set; }

		public Settings()
		{
		}

	}
```
    * 要素名が、Sample → SampleDataに変わります
    * 日本語名でも大丈夫
```XML
<?xml version="1.0" encoding="utf-8"?>
<Settings>
	<SampleData>Sample</SampleData>
</Settings>
```

  3) XMLの属性として出力する

```csharp
using System.Xml.Serialization;
	public class Settings: Test2<Settings>
	{
		// 属性として出力
		[XmlAttribute("ID")]
		public int ID { get; set; }

		public Test2()
		{
		}
	}
```

```XML
<?xml version="1.0" encoding="utf-8"?>
<Test2 ID="123" />
```

  4) XMLのルート要素名を変更する

```csharp
[XmlRoot("テスト")]
	public class Test2 : Serial<Test2>
	{
		
		// 属性として出力
		[XmlAttribute("ID")]
		public int ID { get; set; }
		
		// 中略
	}
```

```XML
<?xml version="1.0" encoding="utf-8"?>
<テスト ID="1000" />
```

## <a id="s21"> `Serial<T>` シリアライズ基底クラス</a>
### 使い方
* シリアライズ化したいクラス に　`Serial<T>`クラスを継承する。(Tは自クラス)
    * 必ず `public` なクラスにすること、また引数なしのコンストラクタを実装する必要あり
* メソッド `Save(filename)` を用いてシリアライズできる
* staticメソッド `Load(filename)` を用いてデシリアライズ できる

## <a id='s22'> `SerlList<T>` シリアライズ可能な汎用リスト</a>
### 使い方
* `List<T>` と置き換える。
* `T`はインターフェイスでも良いが、インターフェイスを含むすべての実装クラスを `public` にする必要がある。
* また、実装クラスには、XmlType属性を付けてそのクラス名を明示すること。

## <a id='s23'> `SerlDic<T>` シリアライズ可能な汎用辞書</a>
### 使い方
* `Dictionary<string,T>` と置き換える。
* `T`は実クラス(インターフェイスは未対応)
```csharp
public SerlDic<int> DicObj{get;set;}
```	
XML構造例
```XML
<DicObj>
	<Entry Key="Key1">
		<Value>1</Value>
	</Entry>
</DicObj>
```
Entry , Key ,Value は変更不可
