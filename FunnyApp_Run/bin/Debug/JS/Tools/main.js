
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

function weather(city){
    var city2=s_string.urlencode(city);
    var url="http://api.map.baidu.com/telematics/v3/weather?location="+city2+"&output=json&ak=FGwyoLoXgYjb92dDdZWrfZ7a";
    var restult=s_net.http_get(url,"utf-8");
    var obj=JSON.parse(restult);
    var weather=obj.results[0].weather_data[0].weather;
    return weather;
}

function init(data){
    var weather1=weather("上海");
    var weather2=weather("银川");
    s_ui.status_label_show("status_label","上海："+weather1+"  银川："+weather2);
    //s_ui.msg(weather);
}
s_ui.button_init("b1","控制面板",50,50,100,30,"show_tools","Tools\\控制面板");

s_ui.button_init("b1_2","科学上网",200,50,100,30,"show_tools","Proxy\\index");

s_ui.button_init("b1_3","上传文件",350,50,100,30,"show_tools","Tools\\上传文件");


s_ui.button_init("b2_1","计算器",50,100,100,30,"show_tools","Calculate\\index");
s_ui.button_init("b2_2","加密收藏夹",200,100,100,30,"show_tools2","FunnyFav\\index");
s_ui.button_init("b_clock","闹钟",350,100,100,30,"show_tools2","Clock\\index");

s_ui.button_init("b3_1","FA Java",50,150,100,30,"show_tools2","FA_Java\\index");
s_ui.button_init("b3_2","加密工具",200,150,100,30,"show_tools2","Tools\\加密工具");
s_ui.button_init("b1_5","聊天",350,150,100,30,"show_tools2","Chat\\main");


s_ui.button_init("b_capture","截屏",50,200,100,30,"show_tools2","Tools\\截屏");

s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","test",300,30);
s_ui.status_add("status","status_label","left");


s_ui.Show_Form(550,360);


s_ui.Form_Title("工具大全");

init("");