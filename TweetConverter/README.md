# TweetConverter

ManoTranslatorでは、ハフマン符号化に使う出現確率にツイッターのツイートデータを利用しています。

RTとハッシュタグを削除し、ひらがなのみのテキストデータに変換します。

カタカナや漢字などを変換する機能はないので、事前にひらがなにしておく必要があります。

http://kakasi.namazu.org/index.html.ja  
変換には、kakasiなどのツールを利用するといいです。

コマンドライン処理（パイプ処理）に対応しています。

使用例(バイナリは添付していないので、事前にビルドしておく必要があります)

```
type tweets.txt | TweetConverter.exe > result.txt
```

