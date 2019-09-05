sys.DataGrid_Init("grid1",10,60,750,380);


sys.Web_Init("web1",10,450,750,250);


sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","文件(&F)");
sys.Menu_Item_Add("Menu1","File","Clear","清空数据","clear_data","");

sys.Menu_Add("Menu1","Data","数据(&D)");
sys.Menu_Item_Add("Menu1","Data","data1","相关性样本1","data_init1","");
sys.Menu_Item_Add("Menu1","Data","data2","卡方检验样本","data_init2","");
sys.Menu_Item_Add("Menu1","Data","data3","WOE样本","data_init3","");

sys.Menu_Add("Menu1","Draw","图表(&C)");
sys.Menu_Item_Add("Menu1","Draw","Draw","绘图1","","");
sys.Menu_Item2_Add("Menu1","Draw","Draw","line","绘图","draw_line","");

sys.Menu_Add("Menu1","Calcuate","计算(&C)");
sys.Menu_Item_Add("Menu1","Calcuate","Correlation","相关性","","");
sys.Menu_Item2_Add("Menu1","Calcuate","Correlation","Correlation1","Person","Pearson_Correlation","");
sys.Menu_Item2_Add("Menu1","Calcuate","Correlation","Correlation2","Spearman","Spearman_Correlation","");
sys.Menu_Item_Add("Menu1","Calcuate","Test","检验","","");
sys.Menu_Item2_Add("Menu1","Calcuate","Test","chi_square","卡方检验","chi_square","");


sys.Menu_Item_Add("Menu1","Calcuate","Other","其他","","");
sys.Menu_Item2_Add("Menu1","Calcuate","Other","WOE_IV","WOE_IV","WOE_IV","");


sys.Show_Form(800,800);
sys.Form_Title("Funny_Grid");