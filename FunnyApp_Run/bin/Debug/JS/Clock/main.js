

function run_click(data){

    s_time.init_alaram();
    s_time.set_time_function("tickle");
    s_time.create_alarm_cron("参考消息抓取","tr6_1","0 8 */1 ? * *");
    
    s_time.create_alarm_daily("日记","tr1",16,18);//写日记
    s_time.create_alarm_daily("日报","tr2",17,45);//写日报
    s_time.create_alarm_daily("整点报时","tr3_10",10,0);
    s_time.create_alarm_daily("整点报时","tr3_11",11,0);
    s_time.create_alarm_daily("整点报时","tr3_12",12,0);
    s_time.create_alarm_daily("整点报时","tr3_14",14,0);
    s_time.create_alarm_daily("整点报时","tr3_16",16,0);
    s_time.create_alarm_daily("整点报时","tr3_18",18,0);
    
    s_time.create_alarm_daily("整点报时","tr3_18_10",18,10);
    s_time.create_alarm_daily("整点报时","tr3_18_30",18,30);
    
    s_time.create_alarm_daily("参考消息","tr4",11,45);
    
    s_time.create_alarm_daily("午饭","tr5_1",11,55);
    
    
    
    s_ui.text_set("txt1","启动闹钟！");
    
    s_ui.button_enable("b1_1","0");
}

function sys_event_alarm(data){
    switch(data){
        case "参考消息抓取":
            s_ui.Notification("提醒","参考消息抓取");
            s_ui.Run_JS_Out("Clock\\参考消息.js");
            break;
        case "日记":
            s_ui.Notification("提醒","可以写日记了");
            open_web("https://www.funnyai.com/funnyai/list_diary.php");
            break;
        case "日报":
            s_ui.Notification("提醒","可以写日报了");
            open_web("http://pms.jiangrongxin.com:11006/pms/index.html");
            break;
        case "参考消息":
            s_ui.Notification("提醒","参考消息");
            open_web("http://www.ckxxbao.com/");
            break;
        case "整点报时":
            s_ui.Notification("提醒","整点报时");
            var a=get_hour_minute();
            sys.TTS(a);
            break;
        case "午饭":
            s_ui.Notification("提醒","准备吃午饭");
            sys.TTS("准备吃午饭");
            break;
    }
}

function init(){
    s_sys.value_save("event:time_event","sys_event_alarm");
}

function addZero(i) {
  if (i < 10) {
    i = "0" + i;
  }
  return i;
}

function get_now() {
  var d = new Date();
  var h = addZero(d.getHours());
  var m = addZero(d.getMinutes());
  var s = addZero(d.getSeconds());
  return h + ":" + m + ":" + s;
}


function get_hour_minute() {
  var d = new Date();
  return d.getHours() + "点" + d.getMinutes() + "分";
}




function tickle(){
    s_ui.text_set("txt_timer",get_now());
}



function open_web(data){
    s_ui.Run_App(data,"");
}



s_ui.textbox_init("txt1","提示信息",10,10,600,90);


s_ui.text_init("txt_timer","00:00",10,150,600,30);

s_ui.button_init("b1_1","运行",10,200,200,30,"run_click","0");
s_ui.button_init("b2_1","test",10,250,200,30,"open_web","https://www.funnyai.com/funnyai/list_diary.php");


s_ui.show_form(700,500);

s_ui.Form_Title("闹钟");


init();

run_click("");

//s_ui.ShowInTask(0);

s_ui.Run_JS_Out("Clock\\参考消息.js");

s_ui.Tray_Show(s_sys.path_js()+"\\clock2.ico");

