
var index=0;
var userName="test";

var log_output="";
var log_error="";

String.prototype.replaceAll = function (FindText, RepText) {
    regExp = new RegExp(FindText, "g");
    return this.replace(regExp, RepText);
}


function generate(data) {
    var count=parseInt(s_ui.text_read("txt_param"));
    var strResult="";

    for (var i=0;i<count+1;i++){
        for (var j=0;j<count;j++){
             if (j+i==count){ 
                 strResult+="0";
             }else{
                 strResult+="1";
             }
        }
        if (i<count) strResult+="\r\n";
    }
    
    s_ui.text_set("txt_param1",strResult);
}

function run_join(bLeft){
    s_ui.text_set("txt_result","");

    var strLines=s_ui.text_read("txt_param1");
    strLines=strLines.replaceAll("\r\n","\n");
    
    var strLines2=s_ui.text_read("txt_param2");
    strLines2=strLines2.replaceAll("\r\n","\n");
    
    var strSplit=strLines.split("\n");
    var output="";
    for (var i=0;i<strSplit.length;i++){
        var strSplit2=strLines2.split("\n");
        for (var j=0;j<strSplit2.length;j++){
            if (bLeft=="1"){
                output+=strSplit[i]+","+strSplit2[j]+"\r\n";
             }else{
                 
                sys.Log(strSplit[i]);
                output+=strSplit2[j]+","+strSplit[i]+"\r\n";
             }
        }
    }
    s_ui.text_set("txt_result",output);
}


s_ui.label_init("lb1","参数：",10,10);
s_ui.text_init("txt_param","28",100,10,100,150);
s_ui.textbox_init("txt_param1","",100,50,300,300);
s_ui.textbox_init("txt_param2","21\r\n22",500,50,100,300);
s_ui.textbox_init("txt_result","",100,400,300,300);

s_ui.button_init("b2_1","生成",500,10,100,30,"generate","");

s_ui.button_init("b2_2","Join左右",650,200,100,30,"run_join","1");

s_ui.button_init("b2_3","Join右左",650,250,100,30,"run_join","0");

s_ui.show_form(800,800);

s_ui.Form_Title("参数生成");






