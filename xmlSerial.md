# <a id="c2">XMLシリアライズ・デシリアライズ</a>

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
