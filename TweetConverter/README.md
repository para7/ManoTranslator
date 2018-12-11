# TweetConverter

ManoTranslatorでは、ハフマン符号化に使う出現確率にツイッターのツイートデータを利用しています。

ツイート履歴のRTとハッシュタグを削除し、欲しい文字だけにフィルタリングします。

カタカナや漢字などは別のツールで事前にひらがなにしておく必要があります。

kakasi  
http://kakasi.namazu.org/index.html.ja  



CLI上での変換メモ

```
TweetConverter.exe < tweet.txt > result.txt
```

