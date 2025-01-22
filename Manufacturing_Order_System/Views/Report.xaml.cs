using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using ScottPlot;
using ScottPlot.Plottables;
using Manufacturing_Order_System.ViewModels;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;


namespace Manufacturing_Order_System
{
    public partial class Report : Page
    {
        MySQLManager manager = new MySQLManager();
        private ReportViewModel viewModel;
        public Report()
        {
            InitializeComponent();
            manager.Initialize();
            viewModel = new ReportViewModel();
            DataContext = viewModel;
            Loaded += Report_Loaded;

        }

        private void Report_Loaded(object sender, EventArgs e)
        {
            double[] graphproduction = new double[5];
            double[] graphdefective = new double[5];
            if (manager.OpenMySqlConnection() == true)
            {
                try
                {
                    // 주문 정보 가져오기
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
                    mySqlDataAdapter.SelectCommand = new MySqlCommand("select SUM(CASE WHEN product_type_id = 1 and product_manufacture_date = curdate() then 1 ELSE 0 END) AS ProductA," +
                        "SUM(CASE WHEN product_type_id = 2 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS ProductB," +
                        "SUM(CASE WHEN product_type_id = 3 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS ProductC," +
                        " SUM(CASE WHEN product_type_id = 4 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS ProductD, " +
                        "SUM(CASE WHEN product_type_id = 5 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS ProductE, " +
                        "SUM(CASE WHEN product_defective_status = 1 AND product_type_id = 1 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS DefectiveA, " +
                        "SUM(CASE WHEN product_defective_status = 1 AND product_type_id = 2 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS DefectiveB, " +
                        "SUM(CASE WHEN product_defective_status = 1 AND product_type_id = 3 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS DefectiveC, " +
                        "SUM(CASE WHEN product_defective_status = 1 AND product_type_id = 4 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS DefectiveD," +
                        "SUM(CASE WHEN product_defective_status = 1 AND product_type_id = 5 and product_manufacture_date = curdate() THEN 1 ELSE 0 END) AS DefectiveE from wpf.product;", App.connection);
                    viewModel.ProductTable.Clear();
                    DataTable dataTable = new DataTable();
                    mySqlDataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int productA = Convert.ToInt32(row["ProductA"]);
                        int productB = Convert.ToInt32(row["ProductB"]);
                        int productC = Convert.ToInt32(row["ProductC"]);
                        int productD = Convert.ToInt32(row["ProductD"]);
                        int productE = Convert.ToInt32(row["ProductE"]);
                        int defectiveA = Convert.ToInt32(row["DefectiveA"]);
                        int defectiveB = Convert.ToInt32(row["DefectiveB"]);
                        int defectiveC = Convert.ToInt32(row["DefectiveC"]);
                        int defectiveD = Convert.ToInt32(row["DefectiveD"]);
                        int defectiveE = Convert.ToInt32(row["DefectiveE"]);
                        graphproduction[0] = (Convert.ToDouble(productA));
                        graphproduction[1] = (Convert.ToDouble(productB));
                        graphproduction[2] = (Convert.ToDouble(productC));
                        graphproduction[3] = (Convert.ToDouble(productD));
                        graphproduction[4] = (Convert.ToDouble(productE));
                        graphdefective[0] = (Convert.ToDouble(defectiveA));
                        graphdefective[1] = (Convert.ToDouble(defectiveB));
                        graphdefective[2] = (Convert.ToDouble(defectiveC));
                        graphdefective[3] = (Convert.ToDouble(defectiveD));
                        graphdefective[4] = (Convert.ToDouble(defectiveE));

                        viewModel.ProductTable.Add(new ProductData
                        {
                            Name = "생산량",
                            ProductA = productA,
                            ProductB = productB,
                            ProductC = productC,
                            ProductD = productD,
                            ProductE = productE,
                        });

                        viewModel.ProductTable.Add(new ProductData
                        {
                            Name = "불량품 수",
                            ProductA = defectiveA,
                            ProductB = defectiveB,
                            ProductC = defectiveC,
                            ProductD = defectiveD,
                            ProductE = defectiveE
                        });
                        viewModel.ProductTable.Add(new ProductData
                        {
                            Name = "합격품 수",
                            ProductA = productA - defectiveA,
                            ProductB = productB - defectiveB,
                            ProductC = productC - defectiveC,
                            ProductD = productD - defectiveD,
                            ProductE = productE - defectiveE
                        });
                        viewModel.ProductTable.Add(new ProductData
                        {
                            Name = "불량률(%)",
                            ProductA = defectiveA != 0
                            ? Convert.ToInt32(Convert.ToDouble(defectiveA) / Convert.ToDouble(productB) * 100)
                            : default,
                            ProductB = defectiveB != 0
                            ? Convert.ToInt32(Convert.ToDouble(defectiveB) / Convert.ToDouble(productB) * 100)
                            : default,
                            ProductC = defectiveC != 0
                            ? Convert.ToInt32(Convert.ToDouble(defectiveC) / Convert.ToDouble(productC) * 100)
                            : default,
                            ProductD = defectiveB != 0
                            ? Convert.ToInt32(Convert.ToDouble(defectiveD) / Convert.ToDouble(productD) * 100)
                            : default,
                            ProductE = defectiveB != 0
                            ? Convert.ToInt32(Convert.ToDouble(defectiveE) / Convert.ToDouble(productE) * 100)
                            : default,
                        });
                    }

                    //판매 수익
                    mySqlDataAdapter.SelectCommand = new MySqlCommand("SELECT pt.product_type_id,pt.product_type_name,COUNT(p.product_id) AS shipped_today_count,(COUNT(p.product_id) * pt.product_type_price) AS revenue FROM product_type pt LEFT JOIN product p ON pt.product_type_id = p.product_type_id WHERE p.product_shipping_date = '2025-01-20' GROUP BY pt.product_type_id, pt.product_type_name, pt.product_type_price;", App.connection);
                    viewModel.revenueDatas.Clear();
                    dataTable.Clear();
                    mySqlDataAdapter.Fill(dataTable);

                    viewModel.revenueDatas.Add(new RevenueData
                    {
                        Name = "판매량",
                        ProductA = Convert.ToInt32(dataTable.Rows[0]["shipped_today_count"]),
                        ProductB = Convert.ToInt32(dataTable.Rows[1]["shipped_today_count"]),
                        ProductC = Convert.ToInt32(dataTable.Rows[2]["shipped_today_count"]),
                        ProductD = Convert.ToInt32(dataTable.Rows[3]["shipped_today_count"]),
                        ProductE = Convert.ToInt32(dataTable.Rows[4]["shipped_today_count"]),
                    });

                    viewModel.revenueDatas.Add(new RevenueData
                    {
                        Name = "판매 수익(원)",
                        ProductA = Convert.ToInt32(dataTable.Rows[0]["revenue"]),
                        ProductB = Convert.ToInt32(dataTable.Rows[1]["revenue"]),
                        ProductC = Convert.ToInt32(dataTable.Rows[2]["revenue"]),
                        ProductD = Convert.ToInt32(dataTable.Rows[3]["revenue"]),
                        ProductE = Convert.ToInt32(dataTable.Rows[4]["revenue"]),
                    });

                    viewModel.TotalRevenue = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        viewModel.TotalRevenue += Convert.ToInt32(dataTable.Rows[i]["revenue"]);
                    }

                    // 막대그래프 그리기
                    barplot.Plot.Add.Bars(graphproduction);
                    barplot.Plot.Add.Bars(graphdefective);

                    Tick[] tick =
                    {
                    new(0, "Product A"),
                    new(1, "Product B"),
                    new(2, "Product C"),
                    new(3, "Product D"),
                    new(4, "Product E"),
                };
                    barplot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(tick);
                    barplot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
                    barplot.Plot.HideGrid();
                    // adjust axis limits so there is no padding below the bar graph
                    barplot.Plot.Axes.Margins(bottom: 0);

                    barplot.Refresh();

                    App.connection.Close();
                }
                catch(InvalidCastException)
                {
                    MessageBox.Show("오늘 생산된 제품이 없습니다.");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    App.connection.Close();
                }
                
            }

        }
        private void SaveDataGridToCsv(DataGrid dataGrid, string filePath)
        {
            if (dataGrid.ItemsSource == null)
                throw new InvalidOperationException("DataGrid에 데이터가 없습니다.");

            StringBuilder csvContent = new StringBuilder();

            // 컬럼 헤더 작성
            var headers = dataGrid.Columns.Select(col => col.Header.ToString()).ToList();
            csvContent.AppendLine(string.Join(",", headers));

            // 데이터 행 작성
            foreach (var item in dataGrid.Items)
            {
                var row = new object[headers.Count];
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    // Cell 값을 가져옴
                    var binding = dataGrid.Columns[i].ClipboardContentBinding as System.Windows.Data.Binding;
                    if (binding != null)
                    {
                        var propertyName = binding.Path.Path;
                        var property = item.GetType().GetProperty(propertyName);
                        row[i] = property?.GetValue(item)?.ToString() ?? string.Empty;
                    }
                }

                csvContent.AppendLine(string.Join(",", row));
            }

            // 파일로 저장
            File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
        }

        // 파일 저장 경로를 선택하는 메서드
        private string? GetSaveFilePath(string defaultFileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = defaultFileName, // 기본 파일 이름
                DefaultExt = ".csv",        // 파일 확장자
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*" // 파일 형식 필터
            };

            // 사용자 선택 후 경로 반환
            bool? result = saveFileDialog.ShowDialog();
            return result == true ? saveFileDialog.FileName : null;
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 파일 저장 경로를 선택할 수 있도록 SaveFileDialog 사용
                string dailyProductFilePath = GetSaveFilePath("Daily Product Table.csv");
                string dailySalesFilePath = GetSaveFilePath("Daily Sales Table.csv");

                if (string.IsNullOrEmpty(dailyProductFilePath) || string.IsNullOrEmpty(dailySalesFilePath))
                {
                    MessageBox.Show("파일 저장이 취소되었습니다.", "저장 취소", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 각각 DataGrid를 선택한 경로로 CSV 파일로 저장
                SaveDataGridToCsv(DailyProductTable, dailyProductFilePath);
                SaveDataGridToCsv(DailySalesTable, dailySalesFilePath);

                MessageBox.Show("CSV 파일이 저장되었습니다.\n\n" +
                                $"Daily Product Table: {dailyProductFilePath}\n" +
                                $"Daily Sales Table: {dailySalesFilePath}",
                                "저장 완료", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }     
    }
}
