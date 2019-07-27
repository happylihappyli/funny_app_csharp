
function show_control(data){
    
    sys.Run_JS("控制面板.js");
}

function show_proxy(data){
    sys.Run_App("E:\\CloudStation\\Robot5\\happyli\\Tools\\http_over_ssh\\FunnyApp.exe","");
}

sys.Add_Button("b1","控制面板",10,50,100,30,"show_control","");
sys.Add_Button("b2","科学上网",200,50,100,30,"show_proxy","");

sys.Show_Form(600,300);


sys.Form_Title("工具大全");