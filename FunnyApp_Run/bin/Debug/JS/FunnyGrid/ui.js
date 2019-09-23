s_ui.datagrid_init("grid1",10,60,750,380);


s_ui.Web_Init("web1",10,450,750,250);


s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","文件(&F)");
s_ui.Menu_Item_Add("Menu1","File","Clear","清空数据","clear_data","");

s_ui.Menu_Add("Menu1","Data","数据(&D)");
s_ui.Menu_Item_Add("Menu1","Data","data1","相关性样本1","data_init1","");
s_ui.Menu_Item_Add("Menu1","Data","data2","卡方检验样本","data_init2","");
s_ui.Menu_Item_Add("Menu1","Data","data3","WOE样本","data_init3","");

s_ui.Menu_Add("Menu1","Draw","图表(&C)");
s_ui.Menu_Item_Add("Menu1","Draw","Draw","绘图1","","");
s_ui.Menu_Item2_Add("Menu1","Draw","Draw","line","绘图","draw_line","");

s_ui.Menu_Add("Menu1","Calcuate","计算(&C)");
s_ui.Menu_Item_Add("Menu1","Calcuate","Correlation","相关性","","");
s_ui.Menu_Item2_Add("Menu1","Calcuate","Correlation","Correlation1","Person","Pearson_Correlation","");
s_ui.Menu_Item2_Add("Menu1","Calcuate","Correlation","Correlation2","Spearman","Spearman_Correlation","");
s_ui.Menu_Item_Add("Menu1","Calcuate","Test","检验","","");
s_ui.Menu_Item2_Add("Menu1","Calcuate","Test","chi_square","卡方检验","chi_square","");


s_ui.Menu_Item_Add("Menu1","Calcuate","Other","其他","","");
s_ui.Menu_Item2_Add("Menu1","Calcuate","Other","WOE_IV","WOE_IV","WOE_IV","");


s_ui.Show_Form(800,800);
s_ui.Form_Title("Funny_Grid");