//全局变量
var input="";
//按钮点击
function button_click(data){
    input+=data;
    s_ui.text_set("txt1",input);
}
//计算结果
function button_click_cal(data){
    input=s_math.Math_Cal(input);
    s_ui.text_set("txt1",input);
}

//计算器界面
//s_ui.button_init(按钮名称,按钮文字,x,y,width,height,event,event_data);
s_ui.button_init("b7","7",10,60,30,30,"button_click","7");
s_ui.button_init("b8","8",50,60,30,30,"button_click","8");
s_ui.button_init("b9","9",90,60,30,30,"button_click","9");
s_ui.button_init("b_time","*",130,60,30,30,"button_click","*");
s_ui.button_init("b_div","/",170,60,30,30,"button_click","/");

s_ui.button_init("b4","4",10,100,30,30,"button_click","4");
s_ui.button_init("b5","5",50,100,30,30,"button_click","5");
s_ui.button_init("b6","6",90,100,30,30,"button_click","6");
s_ui.button_init("b_minus","-",130,100,30,30,"button_click","-");

s_ui.button_init("b1","1",10,140,30,30,"button_click","1");
s_ui.button_init("b2","2",50,140,30,30,"button_click","2");
s_ui.button_init("b3","3",90,140,30,30,"button_click","3");
s_ui.button_init("b_add","+",130,140,30,30,"button_click","+");

s_ui.button_init("b0","0",10,180,70,30,"button_click","0");
s_ui.button_init("b_dot",".",90,180,30,30,"button_click",".");
s_ui.button_init("b_cal","=",130,180,30,30,"button_click_cal","=");

s_ui.text_init("txt1","0.0",10,10,200,30);

s_ui.Show_Form(300,300);
s_ui.Form_Title("计算器");

s_ui.Tray_Show(sys.Path_JS()+"\\main.ico");


