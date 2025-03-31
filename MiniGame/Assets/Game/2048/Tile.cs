using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell tilecell { get; private set; }
    public int number { get; private set; }
    public bool isLocked { get; set; }
    private Image background;
    private TextMeshProUGUI text;
    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void setState(TileState state, int number)
    { 
        this.state = state;
        this.number = number;
        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }
    public void Spawn(TileCell cell)
    {
        if (this.tilecell != null)
            this.tilecell.tile = null;
        this.tilecell = cell;
        this.tilecell.tile = this;
        transform.position = cell.transform.position;
    }
    public void MoveTo(TileCell cell)
    {
        if (this.tilecell != null)
            this.tilecell.tile = null;
        this.tilecell = cell;
        this.tilecell.tile = this;
        //transform.position = cell.transform.position;
        StartCoroutine(Animate(cell.transform.position,false));
    }
    public void Merge(TileCell cell)
    {
        if (this.tilecell != null)
            this.tilecell.tile = null;
        this.tilecell = null;
        cell.tile.isLocked = true;
        StartCoroutine(Animate(cell.transform.position,true));
    }
    private IEnumerator Animate(Vector3 to,bool isMerging)
    {
        float elasped = 0;
        float duration = 0.1f;
        Vector3 from = transform.position;
        while(elasped<duration)
        { 
            transform.position = Vector3.Lerp(from, to, elasped/duration);
            elasped += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        if(isMerging)
            Destroy(gameObject);
    }
}
