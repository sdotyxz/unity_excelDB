using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot]
public sealed class CardInfo : DatabaseEntry
{
    [XmlElement]
    public string CardNo { get; private set; }
    [XmlElement]
    public string CardName { get; private set; }
    [XmlElement]
    public string Rare { get; private set; }
    [XmlElement]
    public string Type { get; private set; }
    [XmlElement]
    public int Level { get; private set; }
    [XmlElement]
    public string Color { get; private set; }
    [XmlElement]
    public string Linkframe { get; private set; }
    [XmlElement]
    public int Buster { get; private set; }
    [XmlElement]
    public string EffectText { get; private set; }
    [XmlElement]
    public string DescribeText { get; private set; }
    [XmlElement]
    public int Power { get; private set; }
    [XmlElement]
    public int GuardPoint { get; private set; }
    [XmlElement]
    public string Feature1 { get; private set; }
    [XmlElement]
    public string Feature2 { get; private set; }
    [XmlElement]
    public int Strike { get; private set; }
    [XmlElement]
    public string Illustrator { get; private set; }
    [XmlElement]
    public string TextureResource { get; private set; }
}