using System.Windows.Forms;

namespace PropertyGridEx
{
	public partial class CtrlTofu :  PropertyControl
	{

		#region メンバ

		// ラジオボタンの配列
		RadioButton[ ] aryRadios;
		int nSel = -1;

		#endregion

		#region プロパティ

		/// <summary>
		/// 値を設定・取得します
		/// </summary>
		public override object ResultValue
		{
			get
			{
				return nSel;
			}
			set
			{
				if( 0<= (int)value  && (int)value < aryRadios.Length )
				{
					aryRadios[ (int)value ].Checked = true;
					nSel = (int)value;
				}
			}
		}

		public override bool Pause { get ; set; }

		#endregion

		#region コンストラクタ

		public CtrlTofu( )
		{
			InitializeComponent( );
			// ラジオボタンを配列に入れる
			aryRadios = new RadioButton[] { rdo01, rdo02, rdo03 };

			for( int i = 0; i < aryRadios.Length; i++ )
			{
				
				aryRadios[ i ].Tag = i;
				// ラジオボタンが変更されたとき、その値を設定し、プルダウンを閉じる
				aryRadios[ i ].CheckedChanged += ( s, e ) =>
				{
					nSel = (int)((RadioButton)s).Tag;
					CloseAction?.Invoke( s, e );
				};				
			}
		}
		#endregion
	}
}
