using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerialCtrl
{
	#region シリアライズ化基底クラス
	public class Serial<T> where T: new() 
	{
		/// <summary>
		/// 継承先クラスをシリアル化します。
		/// </summary>
		/// <param name="file">ファイル名</param>
		public void Save(string file)

		{
			//XmlSerializerオブジェクトを作成
			//オブジェクトの型を指定する
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);

			// XML書込み設定
			XmlWriterSettings setting = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t"
			};
			//setting.NewLineOnAttributes = true;
			using (XmlWriter writer = XmlWriter.Create(file, setting))
			{
				//シリアル化し、XMLファイルに保存する
				serializer.Serialize(writer, this, ns);
			}
		}
		/// <summary>
		/// XMLファイルを読込、指定したクラスにデシリアライズします。
		/// </summary>
		/// <typeparam name="T">デシリアライズ対象クラス</typeparam>
		/// <param name="file">ファイル名</param>
		/// <returns></returns>
		public static T Load(string file)

		{

			// ファイルがない場合は、空のクラスを返す
			if (File.Exists(file) == false)
			{
				Console.WriteLine("ファイルがないよ");
				return new T();
			}
			// XMLSerializerオブジェクトを生成；
			XmlSerializer serializer = new XmlSerializer(typeof(T));

			//読み込むファイルを開く
			FileStream fs = new FileStream(file, FileMode.Open);
			byte[] bs = new byte[fs.Length];
			fs.Read(bs, 0, bs.Length);
			//ファイルを閉じる
			fs.Close();

			//XMLファイルから読み込み、逆シリアル化する
			T obj = (T)serializer.Deserialize(new MemoryStream(bs));
			
			return obj;
		}

	}

	#endregion

	#region シリアライズ可能な汎用リスト
	/// <summary>
	/// シリアライズ可能な汎用リスト
	/// </summary>
	/// <typeparam name="T">インターフェイスを設定する場合、実装先クラスにXmlType属性をつけてください。</typeparam>
	public class SerlList<T> : List<T>, IXmlSerializable
	{
		//ColorやFontのようなシリアライズできない場合true
		private bool SerializeToStr { get; set; }

		public SerlList()
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
			// 指定された型が文字列から変換できる場合は、シリアライズは文字列変換する。
			SerializeToStr = tc.CanConvertFrom(typeof(string)) & tc.CanConvertTo(typeof(string));
		}

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			// string型でシリアライズしている場合
			SerializeToStr = (reader.AttributeCount > 0) ? bool.Parse(reader.GetAttribute(0)) : false;
			// 空のタグは取得しない。
			if (reader.IsEmptyElement) return;

			

			reader.ReadStartElement();
			try
			{
				// タグ内のタグを順に取得
				while (reader.NodeType != XmlNodeType.EndElement)
				{

					Type type =SerialMethods<T>.CheckElementType(reader.Name, SerializeToStr);
					if (type == null)// 型判定NGの場合、次のタグに進む
					{
						reader.ReadStartElement();
					}
					else
					{
						// 型判定OKの場合、シリアライズし、リストに追加する
						XmlSerializer serializer = new XmlSerializer(type);

						var item = serializer.Deserialize(reader);
						if (SerializeToStr)
						{
							item = SerialMethods<T>.ConvertFromString((string)item);
						}
						Add((T)item);
					}
				}
			}
			finally
			{
				reader.ReadEndElement();
			}

		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{

			if (SerializeToStr)
			{
				writer.WriteAttributeString("ToStr", SerializeToStr.ToString());
				writer.WriteAttributeString("Actual", typeof(T).ToString());

			}
			var ns = new XmlSerializerNamespaces();
			ns.Add(String.Empty, String.Empty);
			// Listの要素を全て処理
			foreach (T item in this)
			{
				object SerItem = item;
				XmlSerializer xs;
				// itemの型でXmlSerializerを生成し、シリアライズ
				if (SerializeToStr)
				{
					xs = new XmlSerializer(typeof(string));
					SerItem = SerialMethods<T>.ConvertToString(item);
				}
				else
				{
					Type t = SerialMethods<T>.CheckWriteType(item);
					xs = new XmlSerializer(t);


				}
				xs.Serialize(writer, SerItem, ns);

			}
		}

		public override string ToString()
		{
			return typeof(T).Name+"("+Count+"個)";
		}

	}

	#endregion

	#region シリアライズ可能な汎用辞書

	public class SerlDic<T> : Dictionary<string, T> , IXmlSerializable
	{
		public SerlDic()
		{
		}

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{

			// 空のタグは取得しない。
			if (reader.IsEmptyElement) return;

			reader.ReadStartElement();
			try
			{
				// タグ内のタグを順に取得
				while (reader.NodeType != XmlNodeType.EndElement)
				{

						// 型判定OKの場合、シリアライズし、リストに追加する
					XmlSerializer serializer = new XmlSerializer(typeof(DictionaryObject));
						// 一旦辞書オブジェクトで受ける
						DictionaryObject item = (DictionaryObject)serializer.Deserialize(reader);

						Add(item.Key, item.Value);
					}
				}
			finally
			{
				reader.ReadEndElement();
			}

		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{

			var ns = new XmlSerializerNamespaces();
			ns.Add(String.Empty, String.Empty);

			// 辞書の要素を全て処理
			foreach (KeyValuePair<string, T> item in this)
			{
				DictionaryObject SerItem = new DictionaryObject(item.Key, item.Value);
				XmlSerializer xs;
				// 辞書オブジェクトの型でXmlSerializerを生成し、シリアライズ
				Type t = SerialMethods<DictionaryObject>.CheckWriteType(item);
				xs = new XmlSerializer(t);
				xs.Serialize(writer, SerItem, ns);

			}
		}
		#region "辞書オブジェクト"
		[XmlRoot("Entry")]
		public class DictionaryObject
		{
			[XmlAttribute("Key")]
			public string Key { get; set; }

			public T Value { get; set; }

			public DictionaryObject() {
				Key = "";
				Value =default(T);
			}
			public DictionaryObject(string p_strKey, T p_objValue)
			{
				Key = p_strKey;
				Value = p_objValue;
			}
		}
		#endregion

	}

	#endregion

	#region シリアライズ用便利メソッド集

	class SerialMethods<T>
	{
		public static Type CheckWriteType( object p_objItem )

		{
			Type retVal = typeof( T );
			// テンプレートがインターフェイスの場合は、インスタンスのクラスを返す。
			if( retVal.IsInterface )
				retVal = p_objItem.GetType( );
			return retVal;
		}

		// タグ名が存在するクラス名かどうか判定し、適合する場合、型を返す

		public static Type CheckElementType( string p_strTagName, bool p_bSerializable )

		{
			if( p_bSerializable )
				return typeof( string );
			Type TType = typeof( T );
			Dictionary<string, string> lstSampleTagNames = new Dictionary<string, string>( );
			// ジェネリックがインターフェイスの場合、
			// アセンブリ内の指定されたインターフェイスが実装されているすべてのtypeを検索する
			if( TType.IsInterface )
			{
				foreach( Type IfType in GetInterfaces( ) )
				{
					// ルート名が定義されている場合、その真名の辞書を追加
					XmlRootAttribute objAttribute =
						( XmlRootAttribute ) Attribute.GetCustomAttribute(
								IfType.GetTypeInfo( ), typeof( XmlRootAttribute )
							);
					if( objAttribute != null )
					{
						lstSampleTagNames.Add( objAttribute.ElementName, IfType.Name );
					}
					else
					{
						// ない場合は、真名をそのまま辞書に追加
						lstSampleTagNames.Add( IfType.Name, IfType.Name );

					}
				}
			}
			else
			{
				// ジェネリックにルート名が定義されている場合、その真名の辞書を追加
				XmlRootAttribute objAttribute =
						( XmlRootAttribute ) Attribute.GetCustomAttribute(
							TType.GetTypeInfo( ), typeof( XmlRootAttribute )
						);
				if( objAttribute != null )
				{
					lstSampleTagNames.Add( objAttribute.ElementName, TType.Name );
				}
				else
				{
					lstSampleTagNames.Add( TType.Name, TType.Name );
				}

			}

			// タグ名辞書にある場合その真名に置き換える
			if( lstSampleTagNames.ContainsKey( p_strTagName ) )
			{
				p_strTagName = lstSampleTagNames[p_strTagName];
			}

			//タグ名からクラス名を取得
			if( p_strTagName == "int" )
				p_strTagName = "Int32";
			string typeName = TType.Namespace + "." + p_strTagName;

			Type retVal = Type.GetType( typeName );
			// 現在の名前空間に存在しないクラスの場合は取得しない。
			if( retVal == null )
				return retVal;
			return retVal;

		}

		public static string ConvertToString( T value )
		{
			return TypeDescriptor.GetConverter( typeof( T ) ).ConvertToString( value );
		}
		public static T ConvertFromString( string value )
		{
			return ( T ) TypeDescriptor.GetConverter( typeof( T ) ).ConvertFromString( value );
		}

		/// <summary>
		/// 現在実行中のコードを格納しているアセンブリ内の指定されたインターフェイスが実装されているすべての Type を返します
		/// </summary>
		public static Type[] GetInterfaces( )
		{
			return Assembly.GetExecutingAssembly( ).GetTypes( ).Where( c => c.GetInterfaces( ).Any( t => t == typeof( T ) ) ).ToArray( );
		}

	}

	#endregion

}
