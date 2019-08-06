

function run_click(data){

    sys.create_alarm_daily("job1","tr1",16,46);//写日记
    sys.Show_Text("txt1","启动闹钟！");
}

function sys_event_alarm(data){
    switch(data){
        case "job1":
            open_web("https://www.funnyai.com/funnyai/list_diary.php");
            break;
            
    }
    sys.Notification("test",data);
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


function tickle(){
    sys.Show_Text("txt_timer",get_now());
}



function open_web(data){
    sys.Run_App(data,"");
}



sys.Add_Text_Multi("txt1","提示信息",10,10,600,90);


sys.Add_Text("txt_timer","00:00",10,150,600,30);

sys.Add_Button("b1_1","运行",10,200,200,30,"run_click","0");
sys.Add_Button("b2_1","test",10,250,200,30,"open_web","https://www.funnyai.com/funnyai/list_diary.php");


sys.Show_Form(700,500);

sys.Form_Title("闹钟");


init();






