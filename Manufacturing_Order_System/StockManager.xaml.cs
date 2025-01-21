﻿using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Manufacturing_Order_System
{
    public partial class StockManager : Window
    {
        private readonly MySQLManager mySQLManager = new MySQLManager();

        public StockManager()
        {
            InitializeComponent();

            // 데이터베이스 초기화 및 연결
            mySQLManager.Initialize();
            if (mySQLManager.OpenMySqlConnection())
            {
                LoadDatabaseData();
            }
            else
            {
                MessageBox.Show("데이터베이스 연결에 실패했습니다.");
            }

            // 제품명 체크박스 이벤트 핸들러
            sm_productA.Checked += FilterProductsAndStatus;
            sm_productB.Checked += FilterProductsAndStatus;
            sm_productC.Checked += FilterProductsAndStatus;
            sm_productA.Unchecked += FilterProductsAndStatus;
            sm_productB.Unchecked += FilterProductsAndStatus;
            sm_productC.Unchecked += FilterProductsAndStatus;

            // 상태 체크박스 이벤트 핸들러
            sm_Unstoring.Checked += FilterProductsAndStatus;
            sm_Storing.Checked += FilterProductsAndStatus;
            sm_Unstoring.Unchecked += FilterProductsAndStatus;
            sm_Storing.Unchecked += FilterProductsAndStatus;

            this.Closed += StockManager_Closed;
        }

        // MySQL 데이터를 DataGrid에 로드
        private void LoadDatabaseData(string orderNumber = "", string productNameFilter = "", string statusFilter = "", DateTime? manufactureDate = null, DateTime? shipmentDate = null)
        {
            try
            {
                string query = "SELECT Status AS 상태, ProductTypeID AS 제품종류, ProductID AS 제품ID, ProductName AS 제품명, " +
                               "ManufactureDate AS 생산일자, ShipmentDate AS 출고일자, OrderID AS 주문ID, " +
                               "CASE WHEN IsDefective = 0 THEN 'X' ELSE 'O' END AS 불량여부 " +
                               "FROM stock_manager1";

                // 필터 추가
                List<string> filters = new List<string>();

                // 주문번호 필터
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    filters.Add($"OrderID = '{orderNumber}'");
                }

                // 제품명 필터
                if (!string.IsNullOrEmpty(productNameFilter))
                {
                    filters.Add($"ProductName IN ({productNameFilter})");
                }

                // 상태 필터
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    filters.Add($"Status IN ({statusFilter})");
                }

                // 생산일자 필터
                if (manufactureDate.HasValue)
                {
                    filters.Add($"ManufactureDate = '{manufactureDate.Value:yyyy-MM-dd}'");
                }

                // 출고일자 필터
                if (shipmentDate.HasValue)
                {
                    filters.Add($"ShipmentDate = '{shipmentDate.Value:yyyy-MM-dd}'");
                }

                if (filters.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", filters);
                }

                // 제품 ID 정렬 추가
                query += " ORDER BY ProductID ASC";

                MySqlCommand cmd = new MySqlCommand(query, App.connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                sm_ProductDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터를 로드하는 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        // 제품명 및 상태 체크박스 상태 변경 이벤트
        private void FilterProductsAndStatus(object sender, RoutedEventArgs e)
        {
            // 제품명 필터링
            List<string> selectedProducts = new List<string>();
            if (sm_productA.IsChecked == true) selectedProducts.Add("'A'");
            if (sm_productB.IsChecked == true) selectedProducts.Add("'B'");
            if (sm_productC.IsChecked == true) selectedProducts.Add("'C'");
            string productNameFilter = string.Join(",", selectedProducts);

            // 상태 필터링
            List<string> selectedStatuses = new List<string>();
            if (sm_Unstoring.IsChecked == true) selectedStatuses.Add("'출고'");
            if (sm_Storing.IsChecked == true) selectedStatuses.Add("'미출고'");
            string statusFilter = string.Join(",", selectedStatuses);

            // 생산일자, 출고일자 필터링
            DateTime? manufactureDate = sm_ManufactureDatePicker.SelectedDate;
            DateTime? shipmentDate = sm_ShipmentDatePicker.SelectedDate;

            // 필터링된 데이터 로드
            LoadDatabaseData(productNameFilter: productNameFilter, statusFilter: statusFilter, manufactureDate: manufactureDate, shipmentDate: shipmentDate);
        }

        private void sm_MDPick_Button_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 생산일자 가져오기
            DateTime? manufactureDate = sm_ManufactureDatePicker.SelectedDate;

            // 생산일자만 필터링
            FilterByManufactureDate(manufactureDate);
        }

        private void sm_MDReset_Button_Click(object sender, RoutedEventArgs e)
        {
            // 생산일자 초기화
            sm_ManufactureDatePicker.SelectedDate = null;

            // 생산일자 필터링을 초기화
            FilterByManufactureDate(null);
        }

        private void sm_SDPick_Button_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 출고일자 가져오기
            DateTime? shipmentDate = sm_ShipmentDatePicker.SelectedDate;

            // 출고일자만 필터링
            FilterByShipmentDate(shipmentDate);
        }

        private void sm_SDReset_Button_Click(object sender, RoutedEventArgs e)
        {
            // 출고일자 초기화
            sm_ShipmentDatePicker.SelectedDate = null;

            // 출고일자 필터링을 초기화
            FilterByShipmentDate(null);
        }

        // 날짜 필터링: 생산일자만 필터링
        private void FilterByManufactureDate(DateTime? manufactureDate)
        {
            // 기존 필터와 날짜 필터가 독립적으로 동작하게 필터링된 데이터를 로드
            LoadDatabaseData(manufactureDate: manufactureDate);
        }

        // 날짜 필터링: 출고일자만 필터링
        private void FilterByShipmentDate(DateTime? shipmentDate)
        {
            // 기존 필터와 날짜 필터가 독립적으로 동작하게 필터링된 데이터를 로드
            LoadDatabaseData(shipmentDate: shipmentDate);
        }

        private void sm_searchOrderNum_Click(object sender, RoutedEventArgs e)
        {
            // 텍스트 박스에서 주문 번호 가져오기
            string orderNumber = sm_orderNum.Text;

            // 주문 번호가 비어 있지 않다면 필터링된 데이터를 로드
            if (!string.IsNullOrEmpty(orderNumber))
            {
                LoadDatabaseData(orderNumber);
            }
            else
            {
                // 주문 번호가 비어있다면 전체 데이터를 로드
                FilterProductsAndStatus(sender, e);
            }
        }

        // Window 닫힐 때 리소스 정리
        private void StockManager_Closed(object sender, EventArgs e)
        {
            if (App.connection != null && App.connection.State == ConnectionState.Open)
            {
                mySQLManager.CloseMySqlConnection();
            }
        }
    }
}