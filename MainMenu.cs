using MetroSet_UI.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LPR381_Project
{
    public partial class MainMenu : MetroSetForm
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnCutting_click(object sender, EventArgs e)
        {
            // Cutting plane example.
            double[,] values = { { -13, -8, 0, 0, 0 }, { 1, 2, 1, 0, 10 }, { 5, 2, 0, 1, 20 } };
            Linear new_cutting = new Linear(values);
            List<List<double[,]>> result = new_cutting.CuttingPlane(values, "max");
            rtbResults.Text = new_cutting.PrintResults(result);


        }
    }
}