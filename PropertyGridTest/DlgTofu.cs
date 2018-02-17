using System;
using System.Windows.Forms;
using PropertyGridEx;

namespace PropertyGridTest
{
	public partial class DlgTofu :  Form , IPropertyDialog
	{
		#region メンバ
		int m_nRetVal = -1;
		RadioButton[] m_aryRadioBtns;
		#endregion

		#region プロパティ
		public object ResultValue
		{
			get
			{
				return m_nRetVal;
			}
			set
			{
				if( 0 <= (int)value && (int)value < m_aryRadioBtns.Length )
				{
					m_aryRadioBtns[( int ) value].Checked = true;
					m_nRetVal = ( int ) value;
				}
			}
		}

		public bool Pause { get; set; } // 例えばこのダイアログに於いてデータを取得中の場合などはtrueをセットする
	
		public string Caption
		{
			get
			{
				return Text;
			}
			set
			{
				Text = value;
			}
		}

		#endregion

		#region コンストラクタ
		public DlgTofu()
		{
			InitializeComponent();
			m_aryRadioBtns = new RadioButton[] { rdoKinu, rdoMomen, rdoAge };
			for(int nCnt = 0; nCnt < m_aryRadioBtns.Length; nCnt++ )
			{
				m_aryRadioBtns[nCnt].Tag = nCnt;
				m_aryRadioBtns[nCnt].CheckedChanged += ( s, e ) =>
				{
					m_nRetVal = (int)( ( RadioButton ) s ).Tag;
				};
			}
			// OKボタンとキャンセルボタンをおした時にダイアログを閉じる
			EventHandler BtnHandler = (s,e)  =>{
				Close( );
			};
			btnOK.Click += BtnHandler;
			btnCancel.Click += BtnHandler;
		}
		#endregion

	}
}
