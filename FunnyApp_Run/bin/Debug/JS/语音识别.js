//全局变量
var input="";
//按钮点击
var bRecord=0;
function b_record_click(data){
    if (bRecord==0){
        bRecord=1;
        sys.SR_Record("D:\\123.wav");
        sys.Button_Backgound("b_record","@record_stop");
    }else{
        bRecord=0;
        sys.Button_Backgound("b_record","@recording");
        b_stop_click("");
    }
}
//计算结果
function b_stop_click(data){
    sys.SR_Stop();
    b_recognize_click("");
}

function b_recognize_click(data){
    var a=sys.SR_Recognize("D:\\123.wav");//,"result");//err_msg
    var obj = JSON.parse(a);
    sys.Set_Text("txt1",obj.result[0]);
    
    var url="http://www.funnyai.com/funnyscript/fs_run_line_shell.php?id="+id+"&user="
+encodeURIComponent(userName)+"&param="+encodeURIComponent(param);
    show_error(url);
    var result=sys.Net_Http_GET(url);
    show_error(result);
}

function init(){
    var APP_ID = "16943825";
    var API_KEY = "s6tcrNFyDTPfrMS7dLq8PxDy";
    var SECRET_KEY = "9UqrO2GDM12mcSSjLGK6WzVkyHmEhnBq";
    sys.SR_Init(APP_ID,API_KEY,SECRET_KEY);
    sys.Button_Backgound("b_record","@recording");
}

function b_read_click(data){
    var a=sys.Get_Text("txt1");
    sys.TTS(a);
}

sys.Add_Button("b_record","",100,100,49,46,"b_record_click","");
sys.Add_Button("b_read","阅读",200,100,49,46,"b_read_click","");

sys.Add_Text_Multi("txt1","点击录制按钮，开始录音，点击停止，开始语音识别！",100,150,300,100);
sys.Add_Text_Multi("txt2","AI回复：",100,300,300,100);

sys.Show_Form(500,500);
sys.Form_Title("语音识别");
init();







