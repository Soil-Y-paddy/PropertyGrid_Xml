# PropertyGrid_Xml
プロパティグリッド用のエディタと使用例、及びXML出力の基底クラス

# プロパティグリッドの拡張

<dl>
<dt>名前空間</dt><dd>PropertyGridEx</dd>
<dt>パス</dt><dd>Lib/PropertyGridEx</dd>
</dl>

### 共通事項
プロパティグリッドコントロール `PropertyGrid` を拡張するための属性やクラスを準備した。
当該コントロールの基本的な使い方は、以下を参照のこと。
* [PropertyGridコントロールの使い方 Dobon.net](https://dobon.net/vb/dotnet/control/propertygrid.html)
* [プロパティ グリッドの表示 msn.microsoft.com](https://msdn.microsoft.com/ja-jp/library/bb165149.aspx?f=255&MSPPError=-2147217396)

## [ファイルを開く]ダイアログを表示させるプロパティエディタと追加属性
<dl>
<dt>エディタ</dt>
<dd>FileOpenEditor</dd>
<dt>追加属性</dt>
<dd>FileOpen</dd>
</dl>

### 使い方

1. 対象のプロパティに `Editor` 属性で、`FileOpenEditor` を指定する。
2. `FileOpen` 属性に、ファイルの種類を指定するフィルタを、`Description` 属性に説明を記述する。

## [フォルダの参照]ダイアログを表示させるプロパティエディタ
<dl>
<dt>エディタ</dt>
<dd>FolderOpenEditor</dd>
</dl>

### 使い方

1. 対象のプロパティに `Editor` 属性で、`FileOpenEditor` を指定してください。
2. `Description` 属性に説明を記述してください。

## コンボボックスをプロパティに設定する汎用コンバータと追加属性
<dl>
<dt>タイプコンバータ</dt>
<dd>StringArrayConverter</dd>
<dt>追加属性</dt>
<dd>StringArray</dd>
</dl>

### 使い方

1. 予め表示する文字列配列を、フォームクラスなどに準備する。
2. 配列変数を、`StringArrayConverter.ArraySet` に設定する。
3. 対象のプロパティに、`TypeConverter` 属性で `StringArrayConverter`を指定する
4. `StringArray` 属性に表示する文字列配列のインデックスを指定する。

## インターフェイスをジェネリックにしたコレクションのためのコレクションエディタ

<dl>
<dt>エディタ</dt>
<dd>InterfaceCollectionEditor</dd>
</dl>

### 使い方
1. インターフェイスを実装したクラスを一つ以上準備する。
2. 対象のプロパティ(List<インターフェイス> 型)に、`Editor` 属性で、`InterfaceCollectionEditor` を指定する。

## プロパティ値を設定するユーザダイアログを表示するプロパティエディタと追加属性

<dl>
<dt>エディタ</dt>
<dd>UserFormEditor</dd>
<dt>追加属性</dt>
<dd>UserFormAttribute</dd>
<dt>ダイアログのインターフェイス</dt>
<dd>IPropertyDialog</dd>
</dl>

### 使い方
1. インターフェイス `IPropertyDialog` を実装した、ダイアログを準備する。
	1. `IPropertyDialog.ResultValue` を通して、プロパティ値をやり取りする。
	2. プロパティ設定に必要な項目はダイアログが表示する前のタイミングで準備する。
	   項目の準備中は、メインフォームから`IPropertyDialog.Pause` をtrueに設定しておく。
	3. `IPropertyDialog.Caption` はダイアログ表示時に、`Description`属性から値をうまく
2. 対象のプロパティに `Editor` 属性で、`UserFormEditor` を指定する。
3. フォームクラスで、ダイアログのインスタンスを、`IPropertyDialog`配列に格納する。
4. 4.の配列を`UserFormEditor.Dialogs｀に設定する
5. 複数の異なるダイアログを使用する場合、`UserCtrlAttribute` 属性で、配列インデックスを指定する。



## プロパティ値を設定するコントロールをインライン(ドロップダウン)表示するプロパティエディタと追加属性

<dl>
<dt>エディタ</dt>
<dd>UserCtrlEditor</dd>
<dt>追加属性</dt>
<dd>UserCtrlAttribute</dd>
<dt>コントロールの基底クラス</dt>
<dd>PropertyControl</dd>
</dl>

### 使い方
1.  `PropetyControl` を継承した、ユーザコントロールを準備する。
	(継承元 `UserControl` を、`PropetyControl` に置き換える )
	1. `PropetyControl.ResultValue` をオーバライドし、プロパティ値をやり取りする。
	2. プロパティ設定に必要な項目はダイアログが表示する前のタイミングで準備する。
	   項目の準備中は、メインフォームから`PropetyControl.Pause` をtrueに設定しておく。
2. 対象のプロパティに `Editor` 属性で、`UserCtrlEditor` を指定する。
3. フォームクラスで、ダイアログのインスタンスを、`IPropertyDialog`配列に格納する。
4. 4.の配列を`UserCtrlEditor.Controls｀に設定する
5. 複数の異なるダイアログを使用する場合、`UserCtrlAttribute` 属性で、配列インデックスを指定する。


# XMLシリアライズ・デシリアライズ

<dl>
<dt>名前空間</dt><dd>XmlSerialCtrl</dd>
<dt>パス</dt><dd>Lib/XmlSerialCtrl/</dd>
</dl>

## Serial<T> シリアライズ基底クラス
### 使い方
*　シリアライズ化したいクラス に　`Serial<T>`クラスを継承する。(Tは自クラス)
   *必ず `public` なクラスにすること、また引数なしのコンストラクタを実装する必要あり
* メソッド `Save(filename)` を用いてシリアライズできる
* staticメソッド `Load(filename)` を用いてデシリアライズ できる

## SerlList<T> シリアライズ可能な汎用リスト
### 使い方
* `List<T>` と置き換える。
* `T`はインターフェイスでも良いが、インターフェイスを含むすべての実装クラスを `public` にする必要がある。
* また、実装クラスには、XmlType属性を付けてそのクラス名を明示すること。


