
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
    //s_ui.msg(restult);
    var obj=JSON.parse(restult);
    var weather=obj.results[0].weather_data[0].weather;
    return weather;
}

function init(data){
    //*
    var weather1=weather("上海");
    var weather2=weather("银川");
    s_ui.status_label_show("status_label","上海："+weather1+"  银川："+weather2);
    //*/
    //s_ui.msg(weather);
}

s_ui.button_init("b_tcp","tcp",50,50,100,30,"show_tools","Sample\\tcp");

s_ui.button_init("b1_2","写字板",200,50,100,30,"show_tools","Sample\\写字板");

s_ui.button_init("b1_3","图像匹配.js",350,50,100,30,"show_tools","Sample\\图像匹配");


s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","...",300,30);
s_ui.status_add("status","status_label","left");


s_ui.show_form(550,360);


s_ui.Form_Title("工具大全");

init("");