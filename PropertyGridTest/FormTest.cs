using PropertyGridEx;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace PropertyGridTest
{
	public partial class FormTest : Form
	{
		#region 定数
		const string ConfFile = "sample.xml";
		#endregion
		#region  メンバ変数

		SampleClass m_objSample = new SampleClass( ); // プロパティ表示用のサンプルクラス

		#region プロパティ選択用のオブジェクト	

		public string[][] m_aryComboTexts = new string[2][];// コンボボックス用の文字列配列　[0]: 艦種[] / [1]: 型名[]
		IPropertyDialog[] m_aryDialogs = new IPropertyDialog[1] { new DlgTofu( ) }; // プロパティダイアログ
		PropertyControl[] m_aryControls = new PropertyControl[1] { new CtrlTofu( ) };// プロパティコントロール

		#endregion

		#region プロパティ選択用のサンプルデータセット

		// 艦種の辞書
		public Dictionary<string, string[]> m_aryFleetTypes = new Dictionary<string, string[]>( )
		{
			{"戦艦", new string[]{"大和型","長門型","伊勢型","扶桑型","金剛型"} },
			{"軽巡洋艦" ,new string[]{"天龍型", "球磨型", "長良型", "川内型","夕張型","最上型" ,"阿賀野型","大淀型"  } },
			{"駆逐艦", new string[]{"神風型","睦月型","吹雪型","暁型","初春型","白露型","朝潮型","陽炎型","夕雲型","秋月型","島風"} }

		};

		#endregion

		#endregion

		#region コンストラクタ

		public FormTest( )
		{
			
			InitializeComponent( );
			// プロパティグリッドのイベント追加
			propertyGrid1.PropertyValueChanged += PropertyGrid1_PropertyValueChanged;

		}

		#endregion

		#region イベント

		private void Form1_Load( object sender, EventArgs e )
		{
			// 設定ファイルのロード
			m_objSample = SampleClass.Load( ConfFile );

			// 0列目の艦種配列を辞書のキーから取得する
			m_aryComboTexts[0] = new string[m_aryFleetTypes.Count];
			int nCnt = 0;
			foreach(var objKVP in m_aryFleetTypes )
			{
				m_aryComboTexts[0][nCnt] = objKVP.Key;
				nCnt++;
			}
			// 艦種プロパティが辞書に存在する場合、型名候補配列に辞書の型名リストを設定する
			if( m_aryFleetTypes.ContainsKey( m_objSample.CategoryName ) )
			{
				m_aryComboTexts[1] = m_aryFleetTypes[m_objSample.CategoryName];
			}


			// コンボボックスの文字列配列を設定する
			StringArrayConverter.ArraySet = m_aryComboTexts;
			// ユーザフォームを設定する
			UserFormEditor.Dialogs = m_aryDialogs;
			// ユーザコントロールを設定する
			UserCtrlEditor.Controls = m_aryControls;

			// プロパティグリッドに表示対象を設定
			propertyGrid1.SelectedObject = m_objSample;
		}

		// プロパティグリッドの値が変更されたときのサンプル
		private void PropertyGrid1_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{

			// 変更されたプロパティ名
			string strPropertyName = e.ChangedItem.PropertyDescriptor.Name;
			if( strPropertyName == Utility.GetName( ( ) => m_objSample.CategoryName ) )
			{
				ChangeClassList( );
			}
		}

		private void FormTest_FormClosing( object sender, FormClosingEventArgs e )
		{
			m_objSample.Save( ConfFile );
		}


		#endregion
		#region メソッド

		/// <summary>
		///  艦種が変わったときに、型名を変更する
		/// </summary>
		private void ChangeClassList( )
		{
			if( m_aryFleetTypes.ContainsKey( m_objSample.CategoryName ) )
			{
				m_aryComboTexts[1] = null;
				m_aryComboTexts[1] = m_aryFleetTypes[m_objSample.CategoryName];
				m_objSample.ClassName = m_aryComboTexts[1][0];
			}

		}

		#endregion

	}
}

