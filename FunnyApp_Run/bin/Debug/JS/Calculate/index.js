//全局变量
var input="";
//按钮点击
function button_click(data){
    input+=data;
    sys.Show_Text("txt1",input);
}
//计算结果
function button_click_cal(data){
    input=sys.Math_Cal(input);
    sys.Show_Text("txt1",input);
}

//计算器界面
//sys.Button_Init(按钮名称,按钮文字,x,y,width,height,event,event_data);
sys.Button_Init("b7","7",10,60,30,30,"button_click","7");
sys.Button_Init("b8","8",50,60,30,30,"button_click","8");
sys.Button_Init("b9","9",90,60,30,30,"button_click","9");
sys.Button_Init("b_time","*",130,60,30,30,"button_click","*");
sys.Button_Init("b_div","/",170,60,30,30,"button_click","/");

sys.Button_Init("b4","4",10,100,30,30,"button_click","4");
sys.Button_Init("b5","5",50,100,30,30,"button_click","5");
sys.Button_Init("b6","6",90,100,30,30,"button_click","6");
sys.Button_Init("b_minus","-",130,100,30,30,"button_click","-");

sys.Button_Init("b1","1",10,140,30,30,"button_click","1");
sys.Button_Init("b2","2",50,140,30,30,"button_click","2");
sys.Button_Init("b3","3",90,140,30,30,"button_click","3");
sys.Button_Init("b_add","+",130,140,30,30,"button_click","+");

sys.Button_Init("b0","0",10,180,70,30,"button_click","0");
sys.Button_Init("b_dot",".",90,180,30,30,"button_click",".");
sys.Button_Init("b_cal","=",130,180,30,30,"button_click_cal","=");

sys.Text_Init("txt1","0.0",10,10,200,30);

sys.Show_Form(300,300);
sys.Form_Title("计算器");

sys.Tray_Show("E:\\CloudStation\\Robot5\\GitHub\\funny_app_csharp\\FunnyApp_Run\\bin\\Debug\\JS\\Calculate\\main.ico");


