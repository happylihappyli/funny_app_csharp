

function run_click(data){

    sys.create_alarm_daily("日记","tr1",16,18);//写日记
    sys.create_alarm_daily("日报","tr2",17,45);//写日报
    sys.create_alarm_daily("整点报时","tr3_10",10,0);
    sys.create_alarm_daily("整点报时","tr3_11",11,0);
    sys.create_alarm_daily("整点报时","tr3_12",12,0);
    sys.create_alarm_daily("整点报时","tr3_14",14,0);
    sys.create_alarm_daily("整点报时","tr3_16",16,0);
    sys.create_alarm_daily("整点报时","tr3_18",18,0);
    
    sys.create_alarm_daily("整点报时","tr3_18_10",18,10);
    sys.create_alarm_daily("整点报时","tr3_18_30",18,30);
    
    sys.create_alarm_daily("参考消息","tr4",11,45);
    
    sys.create_alarm_daily("午饭","tr5_1",11,55);
    
    sys.create_alarm_cron("参考消息抓取","tr6_1","0 8 */1 ? * *");
    
    
    sys.Show_Text("txt1","启动闹钟！");
    
    sys.Button_Enable("b1_1","0");
}

function sys_event_alarm(data){
    switch(data){
        case "参考消息抓取":
            sys.Notification("提醒","参考消息抓取");
            sys.Run_JS_Out("Clock\\参考消息.js");
            break;
        case "日记":
            sys.Notification("提醒","可以写日记了");
            open_web("https://www.funnyai.com/funnyai/list_diary.php");
            break;
        case "日报":
            sys.Notification("提醒","可以写日报了");
            open_web("http://pms.jiangrongxin.com:11006/pms/index.html");
            break;
        case "参考消息":
            sys.Notification("提醒","参考消息");
            open_web("http://www.ckxxbao.com/");
            break;
        case "整点报时":
            sys.Notification("提醒","整点报时");
            var a=get_hour_minute();
            sys.TTS(a);
            break;
        case "午饭":
            sys.Notification("提醒","准备吃午饭");
            sys.TTS("准备吃午饭");
            break;
    }
}

function init(){
    sys.init_alaram();
    sys.set_time_function("tickle");
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
    sys.Show_Text("txt_timer",get_now());
}



function open_web(data){
    sys.Run_App(data,"");
}



sys.TextBox_Init("txt1","提示信息",10,10,600,90);


sys.Text_Init("txt_timer","00:00",10,150,600,30);

sys.Button_Init("b1_1","运行",10,200,200,30,"run_click","0");
sys.Button_Init("b2_1","test",10,250,200,30,"open_web","https://www.funnyai.com/funnyai/list_diary.php");


sys.Show_Form(700,500);

sys.Form_Title("闹钟");


init();

run_click("");

//sys.ShowInTask(0);

sys.Run_JS_Out("Clock\\参考消息.js");
