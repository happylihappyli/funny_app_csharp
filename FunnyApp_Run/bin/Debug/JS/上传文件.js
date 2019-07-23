//全局变量
var input="";
//按钮点击
function upload_click(data){
    sys.Net_Upload("robot5.funnyai.com","root","ts38411T","22","E:\\happyli\\Jar\\line_java\\Line_Java.jar","/root/happyli/Line_Java.jar","set_status");
}

function set_status(data){
    var strSplit=data.split(",");
    sys.Show_ProgressBar("progress1",strSplit[1],strSplit[0]);
}

//计算器界面
//sys.Add_Button(按钮名称,按钮文字,x,y,width,height,event,event_data);
sys.Add_Button("b4_1","upload",10,350,200,30,"upload_click","");

sys.Add_Progress("progress1",10,400,300,30);

sys.Show_Form(300,300);
sys.Form_Title("计算器");







