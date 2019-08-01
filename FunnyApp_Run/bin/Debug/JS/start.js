
function show_tools(data){
    sys.Run_JS(data+".js");
}

sys.Add_Button("b1","控制面板",50,50,100,30,"show_tools","控制面板");

sys.Add_Button("b1_2","科学上网",200,50,100,30,"show_tools","代理");

sys.Add_Button("b1_3","上传文件",350,50,100,30,"show_tools","上传文件");

sys.Add_Button("b2_1","计算器",50,150,100,30,"show_tools","计算器");



sys.Show_Form(600,300);


sys.Form_Title("工具大全");