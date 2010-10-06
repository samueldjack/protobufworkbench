using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Be.Windows.Forms;

namespace ProtoBufWorkbench
{
    /// <summary>
    /// Interaction logic for HexEditor.xaml
    /// </summary>
    public partial class HexEditor : UserControl
    {
        private bool _updatingHexBytes;

        public static readonly DependencyProperty HexBytesProperty =
            DependencyProperty.Register("HexBytes", typeof(byte[]), typeof(HexEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ,HandlePropertyChanged));

        private DynamicByteProvider _byteProvider;

        public HexEditor()
        {
            InitializeComponent();

            var byteCollection = new ByteCollection();
            _byteProvider = new DynamicByteProvider(byteCollection);
            _hexBox.ByteProvider = _byteProvider;

            _byteProvider.Changed += HandleHexBytesChangedInForm;
        }

        public byte[] HexBytes
        {
            get { return (byte[])GetValue(HexBytesProperty); }
            set { SetValue(HexBytesProperty, value); }
        }


        private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HexEditor).HandleHexBytesChanged(e);
        }

        private void HandleHexBytesChanged(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            DoGuardedUpdate(() =>
                                {
                                    _byteProvider.DeleteBytes(0, _byteProvider.Length);
                                    _byteProvider.InsertBytes(0, (byte[]) dependencyPropertyChangedEventArgs.NewValue);
                                });
            _hexBox.Refresh();
        }

        private void HandleHexBytesChangedInForm(object sender, EventArgs e)
        {
            DoGuardedUpdate(() =>
                                {
                                    HexBytes = _byteProvider.Bytes.ToArray();
                                });
        }

        private void DoGuardedUpdate(Action updateActions)
        {
            if (_updatingHexBytes)
            {
                return;
            }

            _updatingHexBytes = true;
            try
            {
                updateActions();
            }
            finally
            {
                _updatingHexBytes = false;
            }
        }
    }
}
