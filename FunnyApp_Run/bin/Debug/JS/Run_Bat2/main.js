
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
    var restult=s_net.http_get(url);
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

s_ui.button_init("b_bat","批量跑任务",50,50,100,30,"show_tools","Run_Bat2\\bat");

s_ui.button_init("b_model_step1","step1",50,100,100,30,"show_tools","Run_Bat2\\step1");

s_ui.button_init("b_model_step2","step2",150,100,100,30,"show_tools","Run_Bat2\\step2");
s_ui.button_init("b_model_step3","step3",250,100,100,30,"show_tools","Run_Bat2\\step3");
s_ui.button_init("b_model_step4","step4",350,100,100,30,"show_tools","Run_Bat2\\step4");
s_ui.button_init("b_model_step5","step5",450,100,100,30,"show_tools","Run_Bat2\\step5");
s_ui.button_init("b_model_step6","step6",50,150,100,30,"show_tools","Run_Bat2\\step6");
s_ui.button_init("b_model_step7","step7",150,150,100,30,"show_tools","Run_Bat2\\step7");



s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","test",300,30);
s_ui.status_add("status","status_label","left");


s_ui.Show_Form(700,500);


s_ui.Form_Title("分析工具2");
