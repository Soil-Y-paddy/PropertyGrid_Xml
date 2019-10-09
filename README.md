# PropertyGrid_Xml
プロパティグリッド用のエディタと使用例、及びXML出力の基底クラス


* 目次
    * [ プロパティグリッドの拡張 ](#c1)
        * [共通事項](#s1)
        * [ `FileOpenEditor` [ファイルを開く]ダイアログを表示させるプロパティエディタ ](#s2)
        * [ `FolderOpenEditor`  [フォルダの参照]ダイアログを表示させるプロパティエディタ ](#s3)
        * [ `StringArrayConverter` コンボボックスをプロパティに設定するコンバータ ](#s4)
        * [ `InterfaceCollectionEditor` インターフェイスコレクションのためのコレクションエディタ ](#s5)
        * [ `UserFormEditor` ユーザダイアログを表示するプロパティエディタ  ](#s6)
        * [  `UserCtrlEditor` ユーザコントロールをインライン(ドロップダウン)表示するプロパティエディタ ](#s7)
    * [ XMLシリアライズ・デシリアライズ ](#c2)
        * [ 基本的な使用方法 ](#s201)
	* [ 属性 ](#s202)
        * [ `Serial<T>` シリアライズ基底クラス ](#s21)
        * [ `SerlList<T>` シリアライズ可能な汎用リスト ](#s22)
        * [ `SerlDic<T>` シリアライズ可能な汎用辞書 ](#s23)
	
# <a id="c1">プロパティグリッドの拡張</a>
    
<dl>
<dt>名前空間</dt><dd>PropertyGridEx</dd>
<dt>パス</dt><dd>Lib/PropertyGridEx</dd>
</dl>

### <a id="s1">共通事項</a>
プロパティグリッドコントロール `PropertyGrid` を拡張するための属性やクラスを準備した。<br>
当該コントロールの基本的な使い方は、以下を参照のこと。
* [PropertyGridコントロールの使い方 Dobon.net](https://dobon.net/vb/dotnet/control/propertygrid.html)
* [プロパティ グリッドの表示 msn.microsoft.com](https://msdn.microsoft.com/ja-jp/library/bb165149.aspx?f=255&MSPPError=-2147217396)

## <a id="s2"> `FileOpenEditor` [ファイルを開く]ダイアログを表示させるプロパティエディタ</a>
<dl>
<dt>エディタ</dt>
<dd>FileOpenEditor</dd>
<dt>追加属性</dt>
<dd>FileOpen</dd>
</dl>

![filename](https://user-images.githubusercontent.com/33775885/36342380-0768fec0-1441-11e8-8a9d-7b62d99b5bc5.png)

### 使い方

1. 対象のプロパティに `Editor` 属性で、`FileOpenEditor` を指定する。
2. `FileOpen` 属性に、ファイルの種類を指定するフィルタを、`Description` 属性に説明を記述する。

## <a id="s3">`FolderOpenEditor` [フォルダの参照]ダイアログを表示させるプロパティエディタ </a>
<dl>
<dt>エディタ</dt>
<dd>FolderOpenEditor</dd>
</dl>

![dirname](https://user-images.githubusercontent.com/33775885/36342378-066c9130-1441-11e8-96be-93cc4068ad11.png)

### 使い方

1. 対象のプロパティに `Editor` 属性で、`FileOpenEditor` を指定する。
2. `Description` 属性に説明を記述する。

## <a id="s4"> `StringArrayConverter` コンボボックスをプロパティに設定するコンバータ</a>
<dl>
<dt>タイプコンバータ</dt>
<dd>StringArrayConverter</dd>
<dt>追加属性</dt>
<dd>StringArray</dd>
</dl>

![combobox](https://user-images.githubusercontent.com/33775885/36342375-060a9a3e-1441-11e8-8c9a-bc67c1202bf2.png)

### 使い方

1. 予め表示する文字列配列を、フォームクラスなどに準備する。
2. 配列変数を、`StringArrayConverter.ArraySet` に設定する。
3. 対象のプロパティに、`TypeConverter` 属性で `StringArrayConverter`を指定する
4. `StringArray` 属性に表示する文字列配列のインデックスを指定する。

## <a id="s5">`InterfaceCollectionEditor` インターフェイスコレクションのためのコレクションエディタ</a>

<dl>
<dt>エディタ</dt>
<dd>InterfaceCollectionEditor</dd>
</dl>

![collectioneditor](https://user-images.githubusercontent.com/33775885/36342374-05c87d20-1441-11e8-932a-93cd232a1d0e.png)

### 使い方
1. インターフェイスを実装したクラスを一つ以上準備する。
2. 対象のプロパティ(例えば `List<インターフェイス> ` 型)に、`Editor` 属性で、`InterfaceCollectionEditor` を指定する。

## <a id="s6"> `UserFormEditor` ユーザダイアログを表示するプロパティエディタ </a>

<dl>
<dt>エディタ</dt>
<dd>UserFormEditor</dd>
<dt>追加属性</dt>
<dd>UserFormAttribute</dd>
<dt>ダイアログのインターフェイス</dt>
<dd>IPropertyDialog</dd>
</dl>

![dialog](https://user-images.githubusercontent.com/33775885/36342377-063a6430-1441-11e8-8d72-08dbbefa0e81.png)

### 使い方
1. インターフェイス `IPropertyDialog` を実装した、ダイアログを準備する。
	1. `IPropertyDialog.ResultValue` を通して、プロパティ値をやり取りする。
	2. プロパティ設定に必要な項目はダイアログが表示する前のタイミングで準備する。
	*   項目の準備中は、メインフォームから`IPropertyDialog.Pause` をtrueに設定しておく。
	3. `IPropertyDialog.Caption` はダイアログ表示時に、`Description`属性値を用いて自動設定されます。
2. 対象のプロパティに `Editor` 属性で、`UserFormEditor` を指定する。
3. フォームクラスで、ダイアログのインスタンスを、`IPropertyDialog`配列に格納する。
4. 3.の配列を`UserFormEditor.Dialogs` に設定する
5. 複数の異なるダイアログを使用する場合、`UserCtrlAttribute` 属性で、配列インデックスを指定する。



## <a id="s7"> `UserCtrlEditor` ユーザコントロールをインライン(ドロップダウン)表示するプロパティエディタ</a>

<dl>
<dt>エディタ</dt>
<dd>UserCtrlEditor</dd>
<dt>追加属性</dt>
<dd>UserCtrlAttribute</dd>
<dt>コントロールの基底クラス</dt>
<dd>PropertyControl</dd>
</dl>

![dropdown](https://user-images.githubusercontent.com/33775885/36342379-069927cc-1441-11e8-952a-5efd5415b2fc.png)

### 使い方
1.  `PropetyControl` を継承した、ユーザコントロールを準備する。
	(継承元 `UserControl` を、`PropetyControl` に置き換える )
	1. `PropetyControl.ResultValue` をオーバライドし、プロパティ値をやり取りする。
	2. プロパティ設定に必要な項目はダイアログが表示する前のタイミングで準備する。
	*   項目の準備中は、メインフォームから`PropetyControl.Pause` をtrueに設定しておく。
	3. ユーザ操作で値の変更が完了した場合など、コントロールを閉じる場合は
	* そのイベント(例えば、ラジオボタンのCheckedChenged など)で、`CloseAction(sender, e)` を呼び出す
	* null チェックも必要 →  ` CloseAction?.Invoke( sender, e ); `
2. 対象のプロパティに `Editor` 属性で、`UserCtrlEditor` を指定する。
3. フォームクラスで、ダイアログのインスタンスを、`IPropertyDialog`配列に格納する。
4. 4.の配列を`UserCtrlEditor.Controls｀に設定する
5. 複数の異なるダイアログを使用する場合、`UserCtrlAttribute` 属性で、配列インデックスを指定する。


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
