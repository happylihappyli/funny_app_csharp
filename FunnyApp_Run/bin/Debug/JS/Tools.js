
function show_tools(data){
    sys.Run_JS(data+".js");
}

function show_tools2(data){
    sys.Run_JS_Out(data+".js");
}

function draw_test(data){
    sys.PictureBox_Draw_Ellipse("pic1",50,50,100,100,"blue",2);
}

function draw_test2(data){
    sys.PictureBox_Draw_Ellipse("pic1",100,50,100,100,"blue",2);
}

function mouse_up(arr){
    var x=arr[0];
    var y=arr[1];
    sys.PictureBox_Draw_Ellipse("pic1",x-50,y-50,100,100,"red",2);
}

sys.Button_Init("b1","控制面板",50,50,100,30,"show_tools","控制面板");

sys.Button_Init("b1_2","科学上网",200,50,100,30,"show_tools","代理");

sys.Button_Init("b1_3","上传文件",350,50,100,30,"show_tools","上传文件");

sys.Button_Init("b1_4","闹钟",500,50,100,30,"show_tools2","闹钟");
sys.Button_Init("b1_5","聊天",650,50,100,30,"show_tools","聊天");

sys.Button_Init("b2_1","计算器",50,150,100,30,"show_tools","计算器");
sys.Button_Init("b2_2","加密收藏夹",200,150,100,30,"show_tools2","FunnyFav\\index");

sys.Button_Init("b2_3","draw",350,150,100,30,"draw_test","");
sys.Button_Init("b2_4","draw",500,150,100,30,"draw_test2","");

sys.PictureBox_Init("pic1",50,200,300,200);
sys.PictureBox_Event("pic1","mouse_up");

sys.Show_Form(800,500);


sys.Form_Title("工具大全");