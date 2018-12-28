\ Program Name: f051-clone.fs
\ Date: Sun Aug 20 14:24:58 AEST 2017
\ Copyright 2017  t.porter <terry@tjporter.com.au>, licensed under the GPL
\ For Mecrisp-Stellaris by Matthias Koch
\ Chip: STM32F051
\ Board: STM32F0 Discovery Board
\ Terminal: e4thcom, Copyright (C) 2013-2017 Manfred Mahlow (GPL'd)  https://wiki.forth-ev.de/doku.php/en:projects:e4thcom
\ Clock: 8 Mhz using the internal STM32F051 RC clock, unless otherwise stated
\ All register names must be CMSIS-SVD compliant
\
\ This Program Does: Print a copy of core and dictionary contents to clone everything into another chip
\
\ Inputs: none
\ Outputs: Intel iHEX file.
\
\ ------------------------------------------------------------------------------------------------------  
\ #r f051-clone.fs
\ ------------------------------------------------------------------------------------------------------  



\ This code is essentialy code by Mattthias Koch in his Mecrisp-Stellaris distribution,
\ file mecrisp-stellaris-2.3.8/lpc1114fn28/hexdump.txt

\ Print a copy of core and dictionary contents to clone everything into another chip.
\ 32KB  = $00008000
\ 64KB  = $00010000
\ 128KB = 00020000
\ Change FLASHSIZE to suit your chip Flash size

$00100000 constant FLASHSIZE

: u.4 ( u -- ) 0 <# # # # # #> type ;
: u.2 ( u -- ) 0 <# # # #> type ;

0 variable hexsegment


: clone ( -- ) cr \ Dumps complete Flash
  base @ hex

  $00000000 hexsegment !
  FLASHSIZE $00000000   \ Complete Flash dump

  do
    \ Check if it would be $FFFF only:
    0               \ Not worthy to print
    i 16 + i do      \ Scan data
      i c@ $FF <> or  \ Set flag if there is a non-$FF byte
    loop

    if
      ." :10" i u.4 ." 00" \ Write record-intro with 4 digits.
      $10                   \ Begin checksum
      i          $FF and +   \ Sum the address bytes 
      i 8 rshift $FF and +    \ separately into the checksum

      i 16 + i do
        i c@ u.2 \ Print data with 2 digits
        i c@ +    \ Sum it up for Checksum
      loop

      negate u.2  \ Write Checksum
      cr
    then

  16 +loop
  ." :00000001FF" cr
  base !
; 


