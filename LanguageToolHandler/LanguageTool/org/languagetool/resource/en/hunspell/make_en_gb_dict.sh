#!/bin/sh
# Get dictionaries from https://github.com/marcoagpinto/aoo-mozilla-en-dict
# Get en_us_wordlist.xml from https://github.com/mozilla-b2g/gaia/raw/master/apps/keyboard/js/imes/latin/dictionaries/en_gb_wordlist.xml

# `unmunch` is a command from the Hunspell library: https://github.com/hunspell/hunspell
# `languagetool-dev-6.3-SNAPSHOT-jar-with-dependencies.jar` is built with 'mvn clean compile assembly:single' in languagetool-dev
# `languagetool.jar` is built with 'mvn clean package -DskipTests' in languagetool
# use full paths in all Java files

unmunch en-GB.dic en-GB.aff > en_GB1.txt
cat en_GB1.txt spelling_merged.txt | java -cp /full/path/to/languagetool.jar:/full/path/to/languagetool-dev-6.3-SNAPSHOT-jar-with-dependencies.jar org.languagetool.dev.archive.WordTokenizer en | sort -u > en_GB.txt
java -cp /full/path/to/languagetool.jar org.languagetool.tools.SpellDictionaryBuilder -i en_GB.txt -info en_GB.info -freq en_gb_wordlist.xml  -o en_GB.dict
