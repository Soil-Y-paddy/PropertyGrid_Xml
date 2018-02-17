using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PropertyGridEx
{

	/*プロパティグリッドのカスタマイズ
	 * 
	 * ★　[ファイルを開く]ダイアログを表示させるプロパティエディタと追加属性
	 * 　　FileOpenEditor , FileOpenAttribute
	 *　ファイルを開くダイアログを表示させる標準のエディタ
	 *　　　System.Windows.Forms.Design.FileNameEditor　
	 *　は存在するが、ファイルを開くダイアログ自体をカスタマイズ出来ない。
	 *　そこで、別途属性を追加することで、使いたいプロパティごとに設定できるようにした。
	 *　　今回は、ファイルの種類を指定するフィルタを属性にした。
	 *　　また、ダイアログタイトルを、説明属性(Discription)から得るようにしている。
	 * 
	 * ★ [フォルダの参照]ダイアログを表示させるプロパティエディタ
	 * 　[フォルダの参照]ダイアログを表示させるエディタは標準ではないので、自作する必要がある。
	 * 　今回は、ダイアログの説明テキストに、説明属性(Discription)から得るようにしている。
	 * 
	 * ★コンボボックスをプロパティに設定する汎用コンバータと追加属性
	 * 　プロパティの値をコンボボックスから選択したい場合に使用できるTypeConverter。
	 * 　複数の候補配列をどうやって動的に変えるか検討した結果、属性に配列の組番号を用意し、
	 * 　その組番号のstatic配列にコンボボックスのアイテムを設定する仕組みを採用している。
	 * 　
	 * ★インターフェイスをジェネリックにしたコレクションのためのコレクションエディタ
	 *   List<>などのコレクションに格納されるアイテムの型が一定の場合は、問題ないが、
	 *   インターフェイスになっていて、複数の異なるクラスが選択できる状態の時、
	 *   「追加」が作用しない。
	 *   そこで、指定されたインターフェイスが実装されたクラス一覧を取得し、
	 *   　　追加時に選べれれるようにした。
	 *  
	 *  ★プロパティ値を設定するユーザダイアログを表示するプロパティエディタと追加属性
	 *   ユーザフォームを指定してプロパティの▼を押すと表示させるようにする。
	 *   複数のフォームを使いまわしできるように配列で持たせ、属性でIDを指定する
	 *   ユーザフォームは IPropertyDialog を実装し、値のやり取りを実現する
	 *   ユーザーフォームはメインフォームで予め生成し、スタティックプロパティに割り当てる。
	 *   
	 *   ★プロパティ値を設定するユーザコントロールを表示するプロパティエディタと追加属性
	 *   ユーザコントロールを指定してプロパティの▼を押すとプルダウンで表示させるようにする。
	 *   複数のコントロールを使いまわしできるように配列で持たせ、属性でIDを指定する
	 *   ユーザフォームは PropertyControl を実装し、値のやり取りを実現する
	 *   ユーザーコントロールはメインフォームで予め生成し、スタティックプロパティに割り当てる。
	 *  
	 *  
	 *  
	*/
	#region [ファイルを開く]プロパティエディタと追加属性
	/// <summary>
	/// UIタイプエディタ
	/// </summary>
	public class FileOpenEditor : UITypeEditor
	{
		#region メンバー変数
		private OpenFileDialog m_dlgFileOpen = null;
		#endregion

		#region コンストラクタ
		public FileOpenEditor() : base()
		{

		}
		#endregion

		#region	メソッド

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object objRetVal = "";

			if (m_dlgFileOpen == null)
			{
				m_dlgFileOpen = new OpenFileDialog();
				FileOpenAttribute objAttributes = context.PropertyDescriptor.Attributes.OfType<FileOpenAttribute>().First();
				if (objAttributes != null)
				{
					m_dlgFileOpen.Filter = objAttributes.Filter;
				}
				else
				{
					m_dlgFileOpen.Filter = "すべてのファイル(*.*)|*.*";

				}
				m_dlgFileOpen.Title = context.PropertyDescriptor.Description + "を取得します。";
				m_dlgFileOpen.InitialDirectory = System.Windows.Forms.Application.StartupPath;

			}
			if (value is string)
			{
				m_dlgFileOpen.FileName = value.ToString();
				objRetVal = value;
			}
			if (m_dlgFileOpen.ShowDialog() == DialogResult.OK)
			{
				objRetVal = m_dlgFileOpen.FileName;
			}

			return objRetVal;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		#endregion
	}
	/// <summary>
	/// FileOpenEditorの追加属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class FileOpenAttribute : Attribute
	{
		public string Filter { get; set; }
		public FileOpenAttribute(string p_strFilter)
		{
			Filter = p_strFilter;
		}

	}


	#endregion

	#region  [フォルダの参照]プロパティエディタ
	public class FolderOpenEditor : UITypeEditor
	{

		#region メンバ
		FolderBrowserDialog m_dlgFolderBrowser = null;
		#endregion
		#region コンストラクタ
		public FolderOpenEditor() : base() {


		}
		#endregion
		#region	メソッド

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object objRetVal = "";
			if (m_dlgFolderBrowser == null)
			{
				m_dlgFolderBrowser = new FolderBrowserDialog()
				{
					Description = context.PropertyDescriptor.Description + "を選択します。"
				};
			}

			if (value is string)
			{
				m_dlgFolderBrowser.SelectedPath = value.ToString();
				objRetVal = value;
			}
			if (m_dlgFolderBrowser.ShowDialog() == DialogResult.OK)
			{
				objRetVal = m_dlgFolderBrowser.SelectedPath;
			}

			return objRetVal;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		#endregion

	}
	#endregion

	#region コンボボックスをプロパティに設定する汎用コンバータと追加属性

	/// <summary>
	///  プロパティ文字列コンボ
	///  　使い方
	///  　 [TypeConverter(typeof(StringArrayConverter)), StringArray(nId)]
	///     public string String{get;set;}
	///    予め
	///     StringArrayConverter.ArraySet[nId] に string[] を貼り付けておく
	/// </summary>

	public class StringArrayConverter : StringConverter
	{
		public static string[][] ArraySet { get; set; }

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			int nId = 0;
			// 追加属性から配列番号を取得
			StringArrayAttribute objAttribute = context.PropertyDescriptor.Attributes.OfType<StringArrayAttribute>().First();
			if (objAttribute != null)
			{
				nId = objAttribute.ArrayId;
				if ( ArraySet == null  || nId >= ArraySet.Length)
				{
					// 面倒なので例外をスルーする
					throw new Exception("２次元配列の行IDが、個数より大きいです");
				}
			}
			return new StandardValuesCollection(ArraySet[nId]);
		}
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class StringArrayAttribute : Attribute
	{
		public int ArrayId { get; set; }
		public StringArrayAttribute(int p_nId)
		{
			ArrayId = p_nId;
		}
	}

	#endregion

	#region インターフェイスをジェネリックにしたコレクションのためのコレクションエディタ
	/// <summary>
	/// System.Designを参照
	/// </summary>
	public class InterfaceCollectionEditor : CollectionEditor
	{
		#region メンバー
		// インターフェイスが実装されたクラスを一覧
		Type[] m_aryImpledTypes;
		#endregion

		public InterfaceCollectionEditor(Type p_Type) : base(p_Type)
		{

			// ジェネリックコレクションで、指定されたインターフェイスが実装されたクラス一覧を取得
			if (p_Type.IsGenericType)
			{
				m_aryImpledTypes = Assembly.GetExecutingAssembly().GetTypes()
						.Where(c => c.GetInterfaces().Any(t => t == p_Type.GenericTypeArguments[0])).ToArray();

				// 無いときは、元のインターフェイスを指定する。
				if (m_aryImpledTypes == null || m_aryImpledTypes.Length == 0)
				{
					m_aryImpledTypes = new Type[] { p_Type.GenericTypeArguments[0] };
				}
			}
			else
			{
				m_aryImpledTypes = new Type[] { p_Type.IsArray? p_Type.GetElementType() : p_Type };
			}
		}
		protected override Type[] CreateNewItemTypes()
		{
			return m_aryImpledTypes;
		}

		protected override Type CreateCollectionItemType()
		{
			return m_aryImpledTypes[0];
		}
	}

	public class DictionaryCollectionEditor : CollectionEditor
	{
		public DictionaryCollectionEditor(Type p_Type) : base(p_Type)
		{

		}

		protected override Type CreateCollectionItemType()
		{
			return  typeof(KeyValuePair<string,string>);
		}

	}

	#endregion

	#region プロパティ値を設定するユーザダイアログを表示するプロパティエディタと追加属性

	public class UserFormEditor : UITypeEditor
	{
		#region スタティックプロパティ
		/// <summary>
		/// サブフォームの配列を設定します。
		/// </summary>
		public static IPropertyDialog[] Dialogs { get; set; }
		#endregion

		#region コンストラクタ
		public UserFormEditor() : base()
		{ }
		#endregion

		#region	メソッド

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object objRetVal = value;
			
			if (Dialogs == null || Dialogs.Length == 0)
			{
				// 面倒なので、汎用例外をスルーします。。。
				throw new Exception("フォームが何も設定されていません。");
			}
			else
			{
				// 追加属性から、使用するフォームを選択する
				int nId = 0;
				UserFormAttributeAttribute objAttribute = Utility.GetAttribute<UserFormAttributeAttribute>( context );
				if(objAttribute != null)
				{
					nId = objAttribute.FormId;
					if(nId >= Dialogs.Length)
					{
						// 面倒なので(ry
						throw new Exception("スタティックダイアログのIDが、個数より大きいです");
					}
				}
				// 追加属性が正常なら表示する。フォームのTagに値を受け渡しする。
				if (nId < Dialogs.Length)
				{
					if (Dialogs[nId].Pause) { 
						MessageBox.Show("ダイアログを準備中です。\nしばらくお待ちください");
					}
					else
					{
						Dialogs[nId].Caption = context.PropertyDescriptor.Description + "を取得します。";
						Dialogs[nId].ResultValue = value;
						if (Dialogs[nId].ShowDialog() == DialogResult.OK)
						{
							objRetVal = Dialogs[nId].ResultValue;
						}
					}
				}
			}

			return objRetVal;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
		#endregion
	}

	/// <summary>
	/// UserFormEditorの追加属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class UserFormAttributeAttribute : Attribute
	{
		public int FormId { get; set; }
		public UserFormAttributeAttribute(int p_nId)
		{
			FormId = p_nId;
		}

	}

	/// <summary>
	/// プロパティダイアログのインターフェイス
	/// </summary>
	public interface IPropertyDialog : IContainerControl
	{
		DialogResult DialogResult { get; set; }
		DialogResult ShowDialog();
		/// <summary>
		/// ダイアログの設定結果を取得・設定します。
		/// </summary>
		object ResultValue { get; set; }
		/// <summary>
		/// ダイアログの表示を一時停止したいときその状態を通知・設定します。
		/// </summary>
		bool Pause { get; set; }
		/// <summary>
		/// ダイアログの説明を設定します。
		/// </summary>
		string Caption { get; set; }

	}

	#endregion

	#region インラインコントロールを用いてプロパティを設定する

	public class UserCtrlEditor : UITypeEditor {
		#region スタティックプロパティ
		/// <summary>
		/// サブフォームの配列を設定します。
		/// </summary>
		public static PropertyControl[] Controls { get; set; }
		#endregion

		#region コンストラクタ
		public UserCtrlEditor( ) : base( )
		{ }
		#endregion

		#region	メソッド

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			// エディタ サービスを取得する
			IWindowsFormsEditorService edSvc =( IWindowsFormsEditorService ) provider.GetService( typeof( IWindowsFormsEditorService ) );

			if( Controls == null || Controls.Length == 0 )
			{
				// 面倒なので、汎用例外をスルーします。。。
				throw new Exception( "フォームが何も設定されていません。" );
			}
			else
			{
				// 追加属性から、使用するフォームを選択する
				int nId = 0;

				UserCtrlAttributeAttribute objAttribute = Utility.GetAttribute<UserCtrlAttributeAttribute>( context );
				if( objAttribute != null )
				{
					nId = objAttribute.CtrlId;
					if( nId >= Controls.Length )
					{
						// 面倒なので(ry
						throw new Exception( "スタティックコントロールのIDが、個数より大きいです" );
					}
				}
				// 追加属性が正常なら表示する。フォームのTagに値を受け渡しする。
				if( nId < Controls.Length )
				{
					if( Controls[nId].Pause )
					{
						MessageBox.Show( "ダイアログを準備中です。\nしばらくお待ちください" );
					}
					else
					{
						Controls[nId].ResultValue = value;
						// 閉じるイベントを設定sる
						Controls[nId].CloseAction = ( s, e ) =>
						{
							edSvc.CloseDropDown( );
						};

						if( !Controls[nId].Pause )
						{
							// エディタ サービスに対し、コントロールをドロップダウンとして 
							// 表示するように指示する
							edSvc.DropDownControl( Controls[nId] );
							value = Controls[nId].ResultValue;
						}
						else
						{
							MessageBox.Show( "準備中です。しばらくお待ちください" );
						}
					}
				}
			}
			// 更新された値を返す
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.Modal;
		}
		#endregion
	}

	/// <summary>
	/// UserFormEditorの追加属性
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class UserCtrlAttributeAttribute : Attribute
	{
		public int CtrlId { get; set; }
		public UserCtrlAttributeAttribute( int p_nId )
		{
			CtrlId = p_nId;
		}
	}

	/// <summary>
	/// プロパティ設定コントロールのインターフェイス
	/// </summary>
	public class PropertyControl : UserControl
	{
		/// <summary>
		/// コントロール内で値の変更が確定し、閉じるときに呼び出します。
		/// </summary>
		public EventHandler CloseAction;

		/// <summary>
		/// 設定結果を取得・設定します。
		/// </summary>
		public virtual object ResultValue { get; set; }

		/// <summary>
		/// コントロールの表示を一時停止したいときその状態を通知・設定します。
		/// </summary>
		public virtual bool Pause { get; set; }

		public PropertyControl( ) : base( )
		{

		}

	}
	#endregion

	#region 数値プロパティ文字列コンボボックスを表示させて、その選択IDを取得させるエディタと追加属性
	/// <summary>
	///  数値プロパティ文字列コンボ
	///  　使い方
	///  　 [Editor(typeof(CommboIndexEditor),typeof(UITypeEditor)),CommboIndex(nId)]
	///     public int Number{get;set;}
	///    予め
	///     ComboIndexEditor.ArraySet[nId] に string[] を貼り付けておく
	/// </summary>
	public class CommboIndexEditor : UITypeEditor
	{

		public static string[][] ArraySet { get; set; }

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{

			if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService s)
			{
				ListBox lsvList = new ListBox();

				int nId = 0;
				// 追加属性から配列番号を取得
				CommboIndexAttribute objAttribute = context.PropertyDescriptor.Attributes.OfType<CommboIndexAttribute>().First();
				if (objAttribute != null)
				{
					nId = objAttribute.ArrayId;
					if (nId >= ArraySet.Length)
					{
						// 面倒なので例外はスルーする
						throw new Exception("スタティックダイアログのIDが、個数より大きいです");
					}
				}


				// リストボックスに項目をセット
				lsvList.Items.AddRange(ArraySet[nId]);

				// リストの項目をindexで設定する.
				if (value is int && ((int)value) > 1)
				{
					lsvList.SelectedIndex = ((int)value) - 1;
				}

				// クリックで閉じるようにする
				EventHandler onclick = (sender, e) =>
				{
					s.CloseDropDown();
				};

				lsvList.Click += onclick;

				// ドロップダウンリストの表示
				s.DropDownControl(lsvList);

				lsvList.Click -= onclick;

				// 選択されていればその値を返す
				value = (lsvList.SelectedIndex > 0) ? lsvList.SelectedIndex + 1 : value;
			}
			return value;
		}
	}


	[AttributeUsage(AttributeTargets.Property)]
	public class CommboIndexAttribute : Attribute
	{
		public int ArrayId { get; set; }
		public CommboIndexAttribute(int p_nId)
		{
			ArrayId = p_nId;
		}
	}

	#endregion

	#region ユーティリティ

	public class Utility
	{
		// 
		// 使い方 PropertyUtil.GetName( () => objHoge.HogeProperty )
		/// <summary>
		/// プロパティの名前を取得する
		/// </summary>
		/// <typeparam name="T">(通常使用しない)</typeparam>
		/// <param name="e"> () => objHoge.HogeProperty のように指定  </param>
		/// <returns> "HogeProperty" が帰ってくる</returns>
		public static string GetName<T>( Expression<Func<T>> e )
		{
			var member = ( MemberExpression ) e.Body;
			return member.Member.Name;
		}


		public static T GetAttribute<T>( ITypeDescriptorContext p_context )
		{
			T objRetVal = default(T);
			if( p_context.PropertyDescriptor.Attributes.OfType<T>( ).Count() > 0 )
			{
				objRetVal = p_context.PropertyDescriptor.Attributes.OfType<T>( ).First( );
			}
			return objRetVal;
		}

	}
	#endregion

}
