
function show_proxy(data){
    sys.Run_App("E:\\CloudStation\\Robot5\\happyli\\Tools\\http_over_ssh\\FunnyApp.exe","");
}

function show_tools(data){
    sys.Run_JS(data+".js");
}

sys.Add_Button("b1","控制面板",10,50,100,30,"show_tools","控制面板");

sys.Add_Button("b1_2","科学上网",160,50,100,30,"show_proxy","");

sys.Add_Button("b1_3","上传文件",310,50,100,30,"show_tools","上传文件");

sys.Add_Button("b2_1","计算器",10,150,100,30,"show_tools","计算器");



sys.Show_Form(600,300);


sys.Form_Title("工具大全");