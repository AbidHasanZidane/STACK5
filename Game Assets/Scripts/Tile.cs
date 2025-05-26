using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }  // Stores the current visual and logical state of the tile
    public TileCell cell { get; private set; }    // Reference to the cell this tile occupies
    public int number { get; private set; }       // The number shown on the tile
    public bool locked { get; set; }              // Lock to prevent multiple merges in one move

    private Image background;                     // UI Image component for background color
    private TextMeshProUGUI text;                 // Text component for displaying the number

    private void Awake()
    {
        // Cache references to UI components
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Sets the tile’s display and number state
    public void SetState(TileState state, int number, bool isPrime = false)
    {
        this.state = state;
        this.number = number;

        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }

    // Assigns the tile to a specific cell without animation
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null; // Remove reference from previous cell
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    // Moves the tile to a new cell with animation
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null; // Clear previous cell
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Handles tile merging animation and state update
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true; // Lock the target tile to prevent multiple merges
        StartCoroutine(Animate(cell.transform.position, true));
    }

    // Smooth animation for moving or merging the tile
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.08f;
        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging)
        {
            Destroy(gameObject); // Remove tile object after merge animation
        }
    }
}
