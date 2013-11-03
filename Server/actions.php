<?php
session_start();
Include_once ('mybase.class.php');

if(!isset($_POST['act'])){echo 'not';return;}

$act=$_POST['act'];
if((count($act)>2)||(count($act)==0)){echo 'not';return;}

if($act==5){
//regis
$c2=$_POST['cap'];
if($_SESSION['captchareg']==''){echo 'not';return;}

if( isset($_SESSION['captchareg']) && strtoupper($_SESSION['captchareg']) == strtoupper($c2) )
{
$_SESSION['captchareg']='';
$mail=trim(base64_decode($_POST['mail']));
$textfield = $mail;
$textfield = str_replace('`','',$textfield);
$textfield = str_replace(',','',$textfield);
$textfield = str_replace('"','',$textfield);
$textfield = str_replace(';','',$textfield);
$textfield = str_replace(':','',$textfield);
$textfield = str_replace("'",'',$textfield);
$textfield = htmlspecialchars($textfield);
$mail=$textfield;
$MyBaseReg = new MyBase();
$b=$MyBaseReg->mmail(base64_encode($mail));
if($b==false){echo 'mail';unset ($MyBaseReg);return;}
$SID =$MyBaseReg->GenSID();
$MyBaseReg->regUser($SID,base64_encode($mail));
//----------
$to      = $mail;
$subject = 'Confirmation email for Cryptomail';
$message = 'http://rjump.net/cmailpub/activate.php?mail='.$mail.'&SID='.$SID;
$headers = 'From: info@rjump.net' . "\r\n" .'X-Mailer: PHP/' . phpversion();

mail($to, $subject, $message, $headers);
//---------
unset ($MyBaseReg);
}else{
echo 'not';
}
return;
}

?>