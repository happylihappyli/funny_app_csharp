[[[..\data\default.js]]]
[[[..\data\common_string.js]]]
[[[..\data\tcp.js]]]
[[[..\data\run_bat_common.js]]]


var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");



var file1=s_sys.value_read("file1");
var file2=s_sys.value_read("file2");
var strSep=s_sys.value_read("sep");
var strFilter=s_sys.value_read("filter");

s_ui.msg(strSep);

s_file.read_begin("file1",file1);
s_file.write_begin("file2",file2,false);

var line=s_file.read_line("file1");
while(line!=null){
    if (strSep=="|") strSep="\\|";
    line=line.replaceAll(strSep,",");
    if (strFilter!=""){
        line=line.replaceAll(strFilter,"");
    }
    
    s_file.write_line("file2",line);
    line=s_file.read_line("file1");
}
s_file.read_end("file1");
s_file.write_end("file2");



