
function show_tools(data){
    s_ui.Run_JS(data+".js");
}

function show_tools2(data){
    s_ui.Run_JS_Out(data+".js");
}

function draw_test(data){
    s_ui.PictureBox_Draw_Ellipse("pic1",50,50,100,100,"blue",2);
}

function draw_test2(data){
    s_ui.PictureBox_Draw_Ellipse("pic1",100,50,100,100,"blue",2);
}

function mouse_up(arr){
    var x=arr[0];
    var y=arr[1];
    s_ui.PictureBox_Draw_Ellipse("pic1",x-50,y-50,100,100,"red",2);
}

s_ui.Button_Init("b1","控制面板",50,50,100,30,"show_tools","Tools\\控制面板");

s_ui.Button_Init("b1_2","科学上网",200,50,100,30,"show_tools","Proxy\\index");

s_ui.Button_Init("b1_3","上传文件",350,50,100,30,"show_tools","Tools\\上传文件");

s_ui.Button_Init("b1_4","闹钟",500,50,100,30,"show_tools2","Clock\\index");
s_ui.Button_Init("b1_5","聊天",650,50,100,30,"show_tools2","聊天");

s_ui.Button_Init("b2_1","计算器",50,100,100,30,"show_tools","Calculate\\index");
s_ui.Button_Init("b2_2","加密收藏夹",200,100,100,30,"show_tools2","FunnyFav\\index");

s_ui.Button_Init("b3_1","FA Java",50,150,100,30,"show_tools2","FA_Java\\index");

s_ui.Button_Init("b3_2","加密工具",200,150,100,30,"show_tools2","Tools\\加密工具");



s_ui.Show_Form(800,300);


s_ui.Form_Title("工具大全");
